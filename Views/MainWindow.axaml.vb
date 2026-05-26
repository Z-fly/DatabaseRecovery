Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Interactivity
Imports Avalonia.Markup.Xaml
Imports Avalonia.Platform.Storage
Imports Avalonia.Threading
Imports DatabaseRecovery.Models
Imports DatabaseRecovery.Services
Imports DatabaseRecovery.ViewModels

Namespace Views

  Public Partial Class MainWindow
    Inherits Window

    Private ReadOnly 案件服务 As New 案件数据库服务()
    Private ReadOnly 解析服务 As New SQLite解析服务()
    Private 正在载入表 As Boolean

    Private ReadOnly Property 界面模型 As MainWindowViewModel
      Get
        Return DirectCast(DataContext, MainWindowViewModel)
      End Get
    End Property

    Public Sub New()
      InitializeComponent()
      AddHandler Opened, AddressOf 窗口打开
    End Sub

    Public Sub InitializeComponent()
      AvaloniaXamlLoader.Load(Me)
#If DEBUG Then
      AttachDevTools()
#End If
    End Sub

    Private Sub 窗口打开(发送者 As Object, 参数 As EventArgs)
      Try
        案件服务.初始化()
        界面模型.案件数据库路径 = 案件服务.数据库路径
        刷新历史记录()
      Catch 异常 As Exception
        界面模型.操作提示 = $"案件数据库初始化失败：{异常.Message}"
      End Try
    End Sub

    Private Sub 刷新历史记录(Optional 应用筛选 As Boolean = False)
      Dim 原始记录列表 = 案件服务.加载历史记录().ToList()
      Dim 记录集合 As IEnumerable(Of 历史记录项) = 原始记录列表

      If 应用筛选 Then
        Dim 开始时间 As DateTime
        Dim 结束时间 As DateTime
        Dim 有开始时间 = Not String.IsNullOrWhiteSpace(界面模型.筛选开始日期)
        Dim 有结束时间 = Not String.IsNullOrWhiteSpace(界面模型.筛选结束日期)
        Dim 关键字 = 界面模型.筛选关键词?.Trim()

        If 有开始时间 AndAlso Not DateTime.TryParse(界面模型.筛选开始日期, 开始时间) Then
          界面模型.操作提示 = "开始日期格式无效，请输入有效日期。"
          Return
        End If
        If 有结束时间 AndAlso Not DateTime.TryParse(界面模型.筛选结束日期, 结束时间) Then
          界面模型.操作提示 = "结束日期格式无效，请输入有效日期。"
          Return
        End If
        If 有结束时间 AndAlso 结束时间.TimeOfDay = TimeSpan.Zero Then
          结束时间 = 结束时间.Date.AddDays(1).AddTicks(-1)
        End If

        记录集合 = 记录集合.Where(
          Function(记录)
            Dim 名称匹配 = String.IsNullOrWhiteSpace(关键字) OrElse
              记录.记录名称.Contains(关键字, StringComparison.OrdinalIgnoreCase)

            Dim 创建时间 As DateTime
            Dim 修改时间 As DateTime
            Dim 创建时间有效 = DateTime.TryParse(记录.创建时间, 创建时间)
            Dim 修改时间有效 = DateTime.TryParse(记录.修改时间, 修改时间)

            Dim 日期匹配 As Boolean = Not (有开始时间 OrElse 有结束时间)
            If 有开始时间 OrElse 有结束时间 Then
              If 创建时间有效 Then
                日期匹配 = 日期匹配 OrElse ((Not 有开始时间 OrElse 创建时间 >= 开始时间) AndAlso (Not 有结束时间 OrElse 创建时间 <= 结束时间))
              End If
              If 修改时间有效 Then
                日期匹配 = 日期匹配 OrElse ((Not 有开始时间 OrElse 修改时间 >= 开始时间) AndAlso (Not 有结束时间 OrElse 修改时间 <= 结束时间))
              End If
            End If

            Return 名称匹配 AndAlso 日期匹配
          End Function)
      End If

      Dim 结果列表 = 记录集合.ToList()
      界面模型.历史记录列表.Clear()
      For Each 记录 In 结果列表
        界面模型.历史记录列表.Add(记录)
      Next

      界面模型.历史记录统计文本 = $"显示前 {结果列表.Count} 条记录（总计 {原始记录列表.Count} 条）"
      If 应用筛选 Then
        界面模型.操作提示 = If(结果列表.Count = 0, "未找到匹配的历史记录。", "筛选完成。")
      Else
        界面模型.操作提示 = String.Empty
      End If
    End Sub

    Private Sub 搜索历史记录_Click(发送者 As Object, 参数 As RoutedEventArgs)
      刷新历史记录(True)
    End Sub

    Private Sub 清空历史筛选_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.筛选开始日期 = String.Empty
      界面模型.筛选结束日期 = String.Empty
      界面模型.筛选关键词 = String.Empty
      界面模型.操作提示 = String.Empty
      刷新历史记录()
    End Sub

    Private Sub 打开关系型数据库_Click(发送者 As Object, 参数 As RoutedEventArgs)
      打开新增窗口("关系型数据库", 界面模型.关系型数据库列表, "SQLite")
    End Sub

    Private Sub 打开非关系型数据库_Click(发送者 As Object, 参数 As RoutedEventArgs)
      打开新增窗口("非关系型数据库", 界面模型.非关系型数据库列表, "MongoDB")
    End Sub

    Private Sub 打开新增窗口(标题 As String,
                       类型列表 As IEnumerable(Of 数据库类型项),
                       默认类型 As String)
      界面模型.可选数据库类型列表.Clear()
      For Each 类型 In 类型列表
        界面模型.可选数据库类型列表.Add(类型)
      Next
      界面模型.新增弹窗标题 = 标题
      界面模型.当前数据库类型 = 界面模型.可选数据库类型列表.FirstOrDefault(Function(类型) 类型.名称 = 默认类型)
      界面模型.表单错误 = String.Empty
      界面模型.显示新增弹窗 = True
    End Sub

    Private Sub 取消新增_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.显示新增弹窗 = False
      界面模型.表单错误 = String.Empty
    End Sub

    Private Sub 打开日志分析_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.操作提示 = "本次实现范围为 SQLite 数据文件预处理与预览。"
    End Sub

    Private Async Sub 选择数据库文件_Click(发送者 As Object, 参数 As RoutedEventArgs)
      Dim 文件类型 = New FilePickerFileType("SQLite 数据库或所有文件") With {
        .Patterns = New List(Of String) From {"*.db", "*.sqlite", "*.sqlite3", "*"}
      }
      Dim 文件列表 = Await StorageProvider.OpenFilePickerAsync(New FilePickerOpenOptions With {
        .Title = "选择 SQLite 数据库文件",
        .AllowMultiple = False,
        .FileTypeFilter = New List(Of FilePickerFileType) From {文件类型}
      })
      If 文件列表.Count > 0 Then
        界面模型.新建源文件路径 = 文件列表(0).Path.LocalPath
      End If
    End Sub

    Private Async Sub 选择保存目录_Click(发送者 As Object, 参数 As RoutedEventArgs)
      Dim 文件夹列表 = Await StorageProvider.OpenFolderPickerAsync(New FolderPickerOpenOptions With {
        .Title = "选择保存位置",
        .AllowMultiple = False
      })
      If 文件夹列表.Count > 0 Then
        界面模型.新建保存位置 = 文件夹列表(0).Path.LocalPath
      End If
    End Sub

    Private Async Sub 开始预处理_Click(发送者 As Object, 参数 As RoutedEventArgs)
      If 界面模型.当前数据库类型 Is Nothing OrElse 界面模型.当前数据库类型.名称 <> "SQLite" Then
        界面模型.表单错误 = "当前测试实现支持 SQLite，请在左侧选择 SQLite。"
        Return
      End If
      If String.IsNullOrWhiteSpace(界面模型.新建记录名称) Then
        界面模型.表单错误 = "请输入记录名称。"
        Return
      End If
      If Not File.Exists(界面模型.新建源文件路径) Then
        界面模型.表单错误 = "请选择存在的 SQLite 数据文件。"
        Return
      End If
      If String.IsNullOrWhiteSpace(界面模型.新建保存位置) Then
        界面模型.表单错误 = "请选择保存位置。"
        Return
      End If

      界面模型.显示新增弹窗 = False
      界面模型.显示页面("预处理")
      界面模型.预处理日志列表.Clear()
      界面模型.处理进度 = 0
      界面模型.处理完成 = False
      界面模型.处理用时 = "00:00:00"
      Dim 计时器 = Stopwatch.StartNew()

      Try
        添加处理日志("预处理任务正在排队，请稍等...", 5)
        Await Task.Delay(35)
        添加处理日志("正在读取 SQLite 数据文件...", 15)
        Dim 表结果 = 解析服务.读取表列表(界面模型.新建源文件路径)
        添加处理日志($"共发现 {表结果.Count} 个数据表。", 25)

        Dim 已处理 As Integer = 0
        For Each 表项 In 表结果
          已处理 += 1
          Dim 百分比 = 25 + CInt(Math.Truncate(55.0 * 已处理 / Math.Max(1, 表结果.Count)))
          添加处理日志($"正在解析表信息：test.{表项.名称}（{表项.记录数量} 条）", 百分比)
          Await Task.Delay(18)
        Next

        Dim 数据源目录 = 解析服务.创建结果目录(
          界面模型.新建保存位置,
          界面模型.新建记录名称,
          界面模型.新建源文件路径)
        添加处理日志($"已生成数据源目录：{数据源目录}", 90)

        Dim 当前时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim 记录 As New 历史记录项 With {
          .记录名称 = 界面模型.新建记录名称,
          .数据库类型 = "SQLite",
          .源文件 = 界面模型.新建源文件路径,
          .数据源目录 = 数据源目录,
          .创建时间 = 当前时间,
          .修改时间 = 当前时间,
          .状态 = "存在"
        }
        案件服务.保存记录(记录)
        刷新历史记录()

        界面模型.当前源文件路径 = 记录.源文件
        界面模型.当前数据源目录 = 记录.数据源目录
        界面模型.当前记录名称 = 记录.记录名称
        填充表列表(表结果)
        添加处理日志($"任务信息已写入：{案件服务.数据库路径}", 100)
        界面模型.当前数据源提示 = $"当前数据源：{记录.记录名称}"
        界面模型.处理完成 = True
      Catch 异常 As Exception
        添加处理日志($"处理失败：{异常.Message}", 界面模型.处理进度)
        界面模型.操作提示 = "SQLite 数据处理失败，请检查所选文件。"
      Finally
        计时器.Stop()
        界面模型.处理用时 = 计时器.Elapsed.ToString("hh\:mm\:ss")
      End Try
    End Sub

    Private Sub 添加处理日志(内容 As String, 进度 As Integer)
      界面模型.预处理日志列表.Add(New 预处理日志项($"[{DateTime.Now:HH:mm:ss}] {内容}"))
      界面模型.处理进度 = 进度
    End Sub

    Private Sub 填充表列表(表结果 As IEnumerable(Of 表信息项))
      界面模型.表列表.Clear()
      For Each 表项 In 表结果
        界面模型.表列表.Add(表项)
      Next
    End Sub

    Private Sub 进入预览_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.显示页面("预览")
      If 界面模型.表列表.Count > 0 Then
        界面模型.当前表 = 界面模型.表列表(0)
        加载数据表(界面模型.当前表)
      End If
    End Sub

    Private Sub 返回首页_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.显示页面("首页")
      界面模型.显示数据源详情 = False
    End Sub

    Private Sub 预览表选择变更(发送者 As Object, 参数 As SelectionChangedEventArgs)
      If 正在载入表 Then Return
      Dim 列表控件 = TryCast(发送者, ListBox)
      Dim 表项 = TryCast(列表控件?.SelectedItem, 表信息项)
      If 表项 IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(界面模型.当前源文件路径) Then
        界面模型.当前表 = 表项
        加载数据表(表项)
      End If
    End Sub

    Private Sub 加载数据表(表项 As 表信息项)
      正在载入表 = True
      Try
        界面模型.字段列表.Clear()
        For Each 字段 In 解析服务.读取字段列表(界面模型.当前源文件路径, 表项.名称)
          界面模型.字段列表.Add(字段)
        Next
        刷新显示字段()
        界面模型.数据行列表.Clear()
        For Each 数据行 In 解析服务.读取数据行(界面模型.当前源文件路径, 表项.名称, 界面模型.字段列表)
          界面模型.数据行列表.Add(数据行)
        Next
        界面模型.操作提示 = $"{表项.名称}：显示前 {界面模型.数据行列表.Count} 条记录"
        If 界面模型.数据行列表.Count > 0 Then
          界面模型.当前选中行 = 界面模型.数据行列表(0)
          更新选中详情(界面模型.当前选中行)
        Else
          界面模型.选中行详情列表.Clear()
        End If
      Catch 异常 As Exception
        界面模型.操作提示 = $"读取表失败：{异常.Message}"
      Finally
        正在载入表 = False
      End Try
    End Sub

    Private Sub 显示表记录_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.显示表记录 = True
      界面模型.显示表结构 = False
    End Sub

    Private Sub 显示表结构_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.显示表记录 = False
      界面模型.显示表结构 = True
    End Sub

    Private Sub 设置列_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.是否显示列设置 = Not 界面模型.是否显示列设置
    End Sub

    Private Sub 字段显示_Click(发送者 As Object, 参数 As RoutedEventArgs)
      Dispatcher.UIThread.Post(
        Sub()
          刷新显示字段()
          For Each 数据行 In 界面模型.数据行列表
            解析服务.更新显示单元格(数据行, 界面模型.字段列表)
          Next
        End Sub)
    End Sub

    Private Sub 刷新显示字段()
      界面模型.显示字段列表.Clear()
      For Each 字段 In 界面模型.字段列表.Where(Function(项目) 项目.是否显示)
        界面模型.显示字段列表.Add(字段)
      Next
    End Sub

    Private Sub 数据行选择变更(发送者 As Object, 参数 As SelectionChangedEventArgs)
      Dim 列表控件 = TryCast(发送者, ListBox)
      Dim 数据行 = TryCast(列表控件?.SelectedItem, 数据行项)
      If 数据行 IsNot Nothing Then
        界面模型.当前选中行 = 数据行
        更新选中详情(数据行)
      End If
    End Sub

    Private Sub 更新选中详情(数据行 As 数据行项)
      界面模型.选中行详情列表.Clear()
      For Each 项目 In 数据行.详情列表
        界面模型.选中行详情列表.Add(项目)
      Next
    End Sub

    Private Async Sub 导出表记录_Click(发送者 As Object, 参数 As RoutedEventArgs)
      If 界面模型.当前表 Is Nothing Then Return
      Dim 文件 = Await StorageProvider.SaveFilePickerAsync(New FilePickerSaveOptions With {
        .Title = "导出表记录",
        .SuggestedFileName = $"{界面模型.当前表.名称}.csv",
        .FileTypeChoices = New List(Of FilePickerFileType) From {
          New FilePickerFileType("CSV 文件") With {.Patterns = New List(Of String) From {"*.csv"}}
        }
      })
      If 文件 IsNot Nothing Then
        解析服务.导出CSV(文件.Path.LocalPath, 界面模型.字段列表, 界面模型.数据行列表)
        界面模型.操作提示 = $"已导出：{文件.Path.LocalPath}"
      End If
    End Sub

    Private Sub 打开历史记录_Click(发送者 As Object, 参数 As RoutedEventArgs)
      Dim 按钮 = TryCast(发送者, Button)
      Dim 记录 = TryCast(按钮?.DataContext, 历史记录项)
      If 记录 Is Nothing Then Return
      Dim 数据库路径 = 查找记录文件(记录)
      If 数据库路径 Is Nothing Then
        界面模型.操作提示 = "原始文件和数据源副本均不存在，无法打开预览。"
        Return
      End If

      Try
        界面模型.当前源文件路径 = 数据库路径
        界面模型.当前数据源目录 = 记录.数据源目录
        界面模型.当前记录名称 = 记录.记录名称
        界面模型.当前数据源提示 = $"当前数据源：{记录.记录名称}"
        填充表列表(解析服务.读取表列表(数据库路径))
        界面模型.显示页面("预览")
        If 界面模型.表列表.Count > 0 Then
          界面模型.当前表 = 界面模型.表列表(0)
          加载数据表(界面模型.当前表)
        End If
      Catch 异常 As Exception
        界面模型.操作提示 = $"无法打开数据源：{异常.Message}"
      End Try
    End Sub

    Private Shared Function 查找记录文件(记录 As 历史记录项) As String
      If File.Exists(记录.源文件) Then Return 记录.源文件
      Dim 备份路径 = Path.Combine(记录.数据源目录, Path.GetFileName(记录.源文件))
      If File.Exists(备份路径) Then Return 备份路径
      Return Nothing
    End Function

    Private Sub 打开数据源详情_Click(发送者 As Object, 参数 As RoutedEventArgs)
      Dim 按钮 = TryCast(发送者, Button)
      界面模型.当前详情记录 = TryCast(按钮?.DataContext, 历史记录项)
      界面模型.显示数据源详情 = 界面模型.当前详情记录 IsNot Nothing
    End Sub

    Private Sub 关闭数据源详情_Click(发送者 As Object, 参数 As RoutedEventArgs)
      界面模型.显示数据源详情 = False
    End Sub

  End Class

End Namespace
