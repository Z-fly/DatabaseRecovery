Imports System.Collections.ObjectModel
Imports DatabaseRecovery.Models
Imports ReactiveUI

Namespace ViewModels

  Public Class MainWindowViewModel
    Inherits ViewModelBase

    Private _当前数据源提示 As String = "未打开数据源"
    Private _案件数据库路径 As String = String.Empty
    Private _显示首页 As Boolean = True
    Private _显示预处理 As Boolean
    Private _显示预览 As Boolean
    Private _显示新增弹窗 As Boolean
    Private _显示数据源详情 As Boolean
    Private _新增弹窗标题 As String = "关系型数据库"
    Private _当前数据库类型 As 数据库类型项
    Private _新建记录名称 As String = "SQLite 数据源"
    Private _新建源文件路径 As String = String.Empty
    Private _新建保存位置 As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "恢复")
    Private _表单错误 As String = String.Empty
    Private _处理进度 As Integer
    Private _处理用时 As String = "00:00:00"
    Private _处理完成 As Boolean
    Private _当前源文件路径 As String = String.Empty
    Private _当前数据源目录 As String = String.Empty
    Private _当前记录名称 As String = String.Empty
    Private _当前表 As 表信息项
    Private _当前选中行 As 数据行项
    Private _是否显示列设置 As Boolean
    Private _是否显示详细视图 As Boolean
    Private _显示表记录 As Boolean = True
    Private _显示表结构 As Boolean
    Private _操作提示 As String = String.Empty
    Private _当前详情记录 As 历史记录项
    Private _筛选开始日期 As String = String.Empty
    Private _筛选结束日期 As String = String.Empty
    Private _筛选关键词 As String = String.Empty

    Public Sub New()
      关系型数据库列表 = New ObservableCollection(Of 数据库类型项) From {
        New 数据库类型项("MySQL", "5.6, 5.7, 8.0, xb, qp, xbstream", "文件 / 文件夹"),
        New 数据库类型项("Oracle", "11g, 12c", "文件 / 文件夹"),
        New 数据库类型项("SQL Server", "2008, 2019", "文件 / 文件夹"),
        New 数据库类型项("Access", "2010, 2013, 2016, 2019", "文件 / 文件夹"),
        New 数据库类型项("SQLite", "3.3", "文件"),
        New 数据库类型项("PostgreSQL", "12, 13, 14, 15, 16, 17", "文件夹"),
        New 数据库类型项("MariaDB", "11.5.2, xb, qp, xbstream", "文件 / 文件夹"),
        New 数据库类型项("Realm", "2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 20", "文件 / 文件夹")
      }
      非关系型数据库列表 = New ObservableCollection(Of 数据库类型项) From {
        New 数据库类型项("MongoDB", "4.2, 5.0, 5.1, 5.2, 5.3, 6.0", "文件 / 文件夹"),
        New 数据库类型项("Redis", "6.0", "文件"),
        New 数据库类型项("LevelDB", "ALL", "文件夹"),
        New 数据库类型项("IndexedDB", "ALL", "文件 / 文件夹")
      }
      日志类型列表 = New ObservableCollection(Of 数据库类型项) From {
        New 数据库类型项("binlog 日志分析", "MySQL 5.6 ~ 8.0", "文件")
      }
    End Sub

    Public ReadOnly Property 软件版本 As String = "V2.9.3.3276"

    Public ReadOnly Property 关系型数据库列表 As ObservableCollection(Of 数据库类型项)

    Public ReadOnly Property 非关系型数据库列表 As ObservableCollection(Of 数据库类型项)

    Public ReadOnly Property 日志类型列表 As ObservableCollection(Of 数据库类型项)

    Public ReadOnly Property 可选数据库类型列表 As New ObservableCollection(Of 数据库类型项)()

    Public ReadOnly Property 历史记录列表 As New ObservableCollection(Of 历史记录项)()

    Public ReadOnly Property 表列表 As New ObservableCollection(Of 表信息项)()

    Public ReadOnly Property 字段列表 As New ObservableCollection(Of 字段信息项)()

    Public ReadOnly Property 显示字段列表 As New ObservableCollection(Of 字段信息项)()

    Public ReadOnly Property 数据行列表 As New ObservableCollection(Of 数据行项)()

    Public ReadOnly Property 预处理日志列表 As New ObservableCollection(Of 预处理日志项)()

    Public ReadOnly Property 选中行详情列表 As New ObservableCollection(Of 键值项)()

    Public Property 当前数据源提示 As String
      Get
        Return _当前数据源提示
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_当前数据源提示, value)
      End Set
    End Property

    Public Property 案件数据库路径 As String
      Get
        Return _案件数据库路径
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_案件数据库路径, value)
      End Set
    End Property

    Public Property 显示首页 As Boolean
      Get
        Return _显示首页
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示首页, value)
      End Set
    End Property

    Public Property 显示预处理 As Boolean
      Get
        Return _显示预处理
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示预处理, value)
      End Set
    End Property

    Public Property 显示预览 As Boolean
      Get
        Return _显示预览
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示预览, value)
      End Set
    End Property

    Public Property 显示新增弹窗 As Boolean
      Get
        Return _显示新增弹窗
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示新增弹窗, value)
      End Set
    End Property

    Public Property 显示数据源详情 As Boolean
      Get
        Return _显示数据源详情
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示数据源详情, value)
      End Set
    End Property

    Public Property 新增弹窗标题 As String
      Get
        Return _新增弹窗标题
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_新增弹窗标题, value)
      End Set
    End Property

    Public Property 当前数据库类型 As 数据库类型项
      Get
        Return _当前数据库类型
      End Get
      Set(value As 数据库类型项)
        Me.RaiseAndSetIfChanged(_当前数据库类型, value)
      End Set
    End Property

    Public Property 新建记录名称 As String
      Get
        Return _新建记录名称
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_新建记录名称, value)
      End Set
    End Property

    Public Property 新建源文件路径 As String
      Get
        Return _新建源文件路径
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_新建源文件路径, value)
      End Set
    End Property

    Public Property 新建保存位置 As String
      Get
        Return _新建保存位置
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_新建保存位置, value)
      End Set
    End Property

    Public Property 表单错误 As String
      Get
        Return _表单错误
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_表单错误, value)
      End Set
    End Property

    Public Property 处理进度 As Integer
      Get
        Return _处理进度
      End Get
      Set(value As Integer)
        Me.RaiseAndSetIfChanged(_处理进度, value)
      End Set
    End Property

    Public Property 处理用时 As String
      Get
        Return _处理用时
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_处理用时, value)
      End Set
    End Property

    Public Property 处理完成 As Boolean
      Get
        Return _处理完成
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_处理完成, value)
      End Set
    End Property

    Public Property 当前源文件路径 As String
      Get
        Return _当前源文件路径
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_当前源文件路径, value)
      End Set
    End Property

    Public Property 当前数据源目录 As String
      Get
        Return _当前数据源目录
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_当前数据源目录, value)
      End Set
    End Property

    Public Property 当前记录名称 As String
      Get
        Return _当前记录名称
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_当前记录名称, value)
      End Set
    End Property

    Public Property 当前表 As 表信息项
      Get
        Return _当前表
      End Get
      Set(value As 表信息项)
        Me.RaiseAndSetIfChanged(_当前表, value)
      End Set
    End Property

    Public Property 当前选中行 As 数据行项
      Get
        Return _当前选中行
      End Get
      Set(value As 数据行项)
        Me.RaiseAndSetIfChanged(_当前选中行, value)
      End Set
    End Property

    Public Property 是否显示列设置 As Boolean
      Get
        Return _是否显示列设置
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_是否显示列设置, value)
      End Set
    End Property

    Public Property 是否显示详细视图 As Boolean
      Get
        Return _是否显示详细视图
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_是否显示详细视图, value)
      End Set
    End Property

    Public Property 显示表记录 As Boolean
      Get
        Return _显示表记录
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示表记录, value)
      End Set
    End Property

    Public Property 显示表结构 As Boolean
      Get
        Return _显示表结构
      End Get
      Set(value As Boolean)
        Me.RaiseAndSetIfChanged(_显示表结构, value)
      End Set
    End Property

    Public Property 操作提示 As String
      Get
        Return _操作提示
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_操作提示, value)
      End Set
    End Property

    Public Property 当前详情记录 As 历史记录项
      Get
        Return _当前详情记录
      End Get
      Set(value As 历史记录项)
        Me.RaiseAndSetIfChanged(_当前详情记录, value)
      End Set
    End Property

    Public Property 筛选开始日期 As String
      Get
        Return _筛选开始日期
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_筛选开始日期, value)
      End Set
    End Property

    Public Property 筛选结束日期 As String
      Get
        Return _筛选结束日期
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_筛选结束日期, value)
      End Set
    End Property

    Public Property 筛选关键词 As String
      Get
        Return _筛选关键词
      End Get
      Set(value As String)
        Me.RaiseAndSetIfChanged(_筛选关键词, value)
      End Set
    End Property

    Public Sub 显示页面(页面 As String)
      显示首页 = 页面 = "首页"
      显示预处理 = 页面 = "预处理"
      显示预览 = 页面 = "预览"
    End Sub

  End Class

End Namespace
