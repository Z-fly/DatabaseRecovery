Imports System.Collections.Generic

Namespace ViewModels

  Public Class MainWindowViewModel
    Inherits ViewModelBase

    Public ReadOnly Property 软件版本 As String = "开源版"

    Public ReadOnly Property 当前数据源提示 As String = "未打开数据源"

    Public ReadOnly Property 工具卡片列表 As IReadOnlyList(Of 工具卡片项)

    Public ReadOnly Property 关系型数据库列表 As IReadOnlyList(Of 数据库类型项)

    Public ReadOnly Property 非关系型数据库列表 As IReadOnlyList(Of 数据库类型项)

    Public ReadOnly Property 日志类型列表 As IReadOnlyList(Of 数据库类型项)

    Public ReadOnly Property 历史记录列表 As IReadOnlyList(Of 历史记录项)

    Public ReadOnly Property 表记录列表 As IReadOnlyList(Of 表记录项)

    Public ReadOnly Property 表结构列表 As IReadOnlyList(Of 表结构项)

    Public ReadOnly Property 日志记录列表 As IReadOnlyList(Of 日志记录项)

    Public ReadOnly Property 预处理日志列表 As IReadOnlyList(Of 预处理日志项)

    Public ReadOnly Property 服务记录列表 As IReadOnlyList(Of 服务记录项)

    Public ReadOnly Property 任务列表 As IReadOnlyList(Of 任务项)

    Public Sub New()
      工具卡片列表 = New List(Of 工具卡片项) From {
        New 工具卡片项("关系型数据库", "支持：MySQL、SQL文档、阿里云高级下载文件、SQL Server、Oracle、PostgreSQL 等", "添加数据库"),
        New 工具卡片项("非关系型数据库", "支持：MongoDB、Redis、LevelDB、IndexedDB 等数据库文件解析", "添加任务"),
        New 工具卡片项("日志分析", "支持：binlog、其他数据库日志的读取、筛选和导出", "打开日志分析")
      }

      关系型数据库列表 = New List(Of 数据库类型项) From {
        New 数据库类型项("MySQL", "5.6、5.7、8.0、xb、qp、xbstream", "文件 / 文件夹"),
        New 数据库类型项("Oracle", "11g、12c", "文件 / 文件夹"),
        New 数据库类型项("SQL Server", "2008、2019", "文件 / 文件夹"),
        New 数据库类型项("Access", "2010、2013、2016、2019", "文件 / 文件夹"),
        New 数据库类型项("SQLite", "3.3", "文件"),
        New 数据库类型项("PostgreSQL", "12、13、14、15、16、17", "文件夹"),
        New 数据库类型项("MariaDB", "11.5.2、xb、qp、xbstream", "文件 / 文件夹"),
        New 数据库类型项("Realm", "2、3、4、5、6、10、11、12、13、14、20", "文件 / 文件夹")
      }

      非关系型数据库列表 = New List(Of 数据库类型项) From {
        New 数据库类型项("MongoDB", "4.2、5.0、5.1、5.2、5.3、6.0", "文件 / 文件夹"),
        New 数据库类型项("Redis", "6.0", "文件"),
        New 数据库类型项("LevelDB", "ALL", "文件夹"),
        New 数据库类型项("IndexedDB", "ALL", "文件 / 文件夹")
      }

      日志类型列表 = New List(Of 数据库类型项) From {
        New 数据库类型项("binlog日志分析", "MySQL 5.6 ~ 8.0", "文件")
      }

      历史记录列表 = New List(Of 历史记录项) From {
        New 历史记录项("mysql", "2023-11-08 10:54:33", "2023-11-08 10:54:36", "存在"),
        New 历史记录项("binlog", "2023-11-08 10:47:39", "2023-11-08 10:47:42", "存在"),
        New 历史记录项("oracle1", "2023-11-08 09:41:00", "2023-11-08 09:41:00", "存在"),
        New 历史记录项("oracle", "2023-11-08 09:39:14", "2023-11-08 09:39:14", "存在"),
        New 历史记录项("ORACLE", "2023-11-06 15:05:54", "2023-11-06 15:05:54", "存在"),
        New 历史记录项("pg14", "2023-11-06 15:01:46", "2023-11-06 15:01:46", "存在"),
        New 历史记录项("access-2019", "2023-11-03 10:50:02", "2023-11-03 10:50:05", "存在")
      }

      表记录列表 = New List(Of 表记录项) From {
        New 表记录项("正常", "1", "6", "1", "9", "折线图", "line", "[190,214]", "[cbd998]", "1000"),
        New 表记录项("正常", "2", "6", "5", "14", "饼图", "pie", "[716b709]", "[304f3f]", "1000"),
        New 表记录项("正常", "3", "6", "6", "15", "柱状图", "bar", "[3cd0fe]", "[293ac5]", "1000"),
        New 表记录项("正常", "4", "6", "4", "12", "关系图", "graph", "null", "null", "0"),
        New 表记录项("正常", "5", "6", "2", "10", "通用分析结果", "graph", "null", "null", "0"),
        New 表记录项("删除", "6", "6", "3", "11", "被删除记录", "graph", "null", "null", "0"),
        New 表记录项("正常", "7", "6", "8", "18", "层级结构图", "hierarchy", "null", "null", "0"),
        New 表记录项("正常", "8", "6", "8", "18", "字段分布", "graph", "null", "null", "0"),
        New 表记录项("删除", "9", "6", "2", "10", "异常记录", "graph", "null", "null", "0"),
        New 表记录项("正常", "10", "6", "3", "11", "坐标数据", "graph", "null", "null", "0")
      }

      表结构列表 = New List(Of 表结构项) From {
        New 表结构项("id", "bigint unsigned", "否", "自增", "主键"),
        New 表结构项("table_id", "int", "否", "0", "表编号"),
        New 表结构项("name", "varchar(255)", "是", "null", "名称"),
        New 表结构项("type", "varchar(50)", "是", "null", "图表类型"),
        New 表结构项("x_axis", "text", "是", "null", "横轴数据"),
        New 表结构项("y_axis", "text", "是", "null", "纵轴数据"),
        New 表结构项("is_deleted", "tinyint", "否", "0", "删除状态")
      }

      日志记录列表 = New List(Of 日志记录项) From {
        New 日志记录项("2023-04-28 15:12:04", "其他(other)", "CREATE DATABASE `soso`", "mysql-bin.000037", "0x0000000000000E"),
        New 日志记录项("2023-04-28 15:12:04", "删除(delete)", "CREATE TABLE `sys_role` (`id` varchar(50), `name` varchar(256))", "mysql-bin.000037", "0x0000000000017F"),
        New 日志记录项("2023-04-28 15:12:04", "删除(delete)", "ALTER TABLE `sys_role` DROP `hlr_ds_delete`", "mysql-bin.000037", "0x000000000002F9"),
        New 日志记录项("2023-04-28 15:12:04", "删除(delete)", "CREATE TABLE `user_third` (`create_time` BIGINT, `product_title` VARCHAR(20))", "mysql-bin.000037", "0x00000000000381"),
        New 日志记录项("2023-04-28 15:12:04", "删除(delete)", "ALTER TABLE `user_third` DROP `hlr_ds_delete`", "mysql-bin.000037", "0x00000000000729"),
        New 日志记录项("2023-04-28 15:12:04", "其他(other)", "INSERT INTO `audit_log` VALUES (...)", "mysql-bin.000038", "0x00000000000952")
      }

      预处理日志列表 = New List(Of 预处理日志项) From {
        New 预处理日志项("[10:54:33] 预处理任务正在排队，请稍等..."),
        New 预处理日志项("[10:54:34] 正在启动解析服务..."),
        New 预处理日志项("[10:54:35] 正在解析表结构信息..."),
        New 预处理日志项("[10:54:36] 正在解析表数据：table.65cms_position_data"),
        New 预处理日志项("[10:54:37] 正在解析表数据：table.chart"),
        New 预处理日志项("[10:54:38] 预处理任务即将完成")
      }

      服务记录列表 = New List(Of 服务记录项) From {
        New 服务记录项("1", "mysql", "内置服务", "MySQL", "已连接", "127.0.0.1:8568 / root / 123456"),
        New 服务记录项("2", "pg14", "自定义服务", "PostgreSQL", "未连接", "127.0.0.1:5432 / postgres"),
        New 服务记录项("3", "oracle1", "内置服务", "Oracle", "已停止", "127.0.0.1:1521 / system")
      }

      任务列表 = New List(Of 任务项) From {
        New 任务项("MySQL 数据预处理", "2023-11-08 10:54:33", "00:00:05", "100%", "成功"),
        New 任务项("MySQL 数据解析", "2023-11-08 10:55:10", "00:01:12", "76%", "运行中"),
        New 任务项("binlog 日志分析", "2023-11-08 10:47:39", "00:00:03", "100%", "成功")
      }
    End Sub

  End Class

  Public Class 工具卡片项

    Public Sub New(标题 As String, 描述 As String, 操作提示 As String)
      Me.标题 = 标题
      Me.描述 = 描述
      Me.操作提示 = 操作提示
    End Sub

    Public ReadOnly Property 标题 As String

    Public ReadOnly Property 描述 As String

    Public ReadOnly Property 操作提示 As String

  End Class

  Public Class 数据库类型项

    Public Sub New(名称 As String, 版本 As String, 来源方式 As String)
      Me.名称 = 名称
      Me.版本 = 版本
      Me.来源方式 = 来源方式
    End Sub

    Public ReadOnly Property 名称 As String

    Public ReadOnly Property 版本 As String

    Public ReadOnly Property 来源方式 As String

  End Class

  Public Class 历史记录项

    Public Sub New(名称 As String, 添加时间 As String, 更新时间 As String, 状态 As String)
      Me.名称 = 名称
      Me.添加时间 = 添加时间
      Me.更新时间 = 更新时间
      Me.状态 = 状态
    End Sub

    Public ReadOnly Property 名称 As String

    Public ReadOnly Property 添加时间 As String

    Public ReadOnly Property 更新时间 As String

    Public ReadOnly Property 状态 As String

  End Class

  Public Class 表记录项

    Public Sub New(状态 As String, 行号 As String, 数据编号 As String, 子编号 As String, 表编号 As String, 名称 As String, 类型 As String, 横轴 As String, 纵轴 As String, 限制值 As String)
      Me.状态 = 状态
      Me.行号 = 行号
      Me.数据编号 = 数据编号
      Me.子编号 = 子编号
      Me.表编号 = 表编号
      Me.名称 = 名称
      Me.类型 = 类型
      Me.横轴 = 横轴
      Me.纵轴 = 纵轴
      Me.限制值 = 限制值
    End Sub

    Public ReadOnly Property 状态 As String

    Public ReadOnly Property 行号 As String

    Public ReadOnly Property 数据编号 As String

    Public ReadOnly Property 子编号 As String

    Public ReadOnly Property 表编号 As String

    Public ReadOnly Property 名称 As String

    Public ReadOnly Property 类型 As String

    Public ReadOnly Property 横轴 As String

    Public ReadOnly Property 纵轴 As String

    Public ReadOnly Property 限制值 As String

  End Class

  Public Class 表结构项

    Public Sub New(字段名称 As String, 字段类型 As String, 允许为空 As String, 默认值 As String, 注释 As String)
      Me.字段名称 = 字段名称
      Me.字段类型 = 字段类型
      Me.允许为空 = 允许为空
      Me.默认值 = 默认值
      Me.注释 = 注释
    End Sub

    Public ReadOnly Property 字段名称 As String

    Public ReadOnly Property 字段类型 As String

    Public ReadOnly Property 允许为空 As String

    Public ReadOnly Property 默认值 As String

    Public ReadOnly Property 注释 As String

  End Class

  Public Class 日志记录项

    Public Sub New(写入时间 As String, 类型 As String, 操作内容 As String, 所属文件 As String, 偏移位置 As String)
      Me.写入时间 = 写入时间
      Me.类型 = 类型
      Me.操作内容 = 操作内容
      Me.所属文件 = 所属文件
      Me.偏移位置 = 偏移位置
    End Sub

    Public ReadOnly Property 写入时间 As String

    Public ReadOnly Property 类型 As String

    Public ReadOnly Property 操作内容 As String

    Public ReadOnly Property 所属文件 As String

    Public ReadOnly Property 偏移位置 As String

  End Class

  Public Class 预处理日志项

    Public Sub New(内容 As String)
      Me.内容 = 内容
    End Sub

    Public ReadOnly Property 内容 As String

  End Class

  Public Class 服务记录项

    Public Sub New(序号 As String, 记录名称 As String, 服务类型 As String, 数据库 As String, 状态 As String, 连接信息 As String)
      Me.序号 = 序号
      Me.记录名称 = 记录名称
      Me.服务类型 = 服务类型
      Me.数据库 = 数据库
      Me.状态 = 状态
      Me.连接信息 = 连接信息
    End Sub

    Public ReadOnly Property 序号 As String

    Public ReadOnly Property 记录名称 As String

    Public ReadOnly Property 服务类型 As String

    Public ReadOnly Property 数据库 As String

    Public ReadOnly Property 状态 As String

    Public ReadOnly Property 连接信息 As String

  End Class

  Public Class 任务项

    Public Sub New(任务名称 As String, 开始时间 As String, 耗时 As String, 进度 As String, 状态 As String)
      Me.任务名称 = 任务名称
      Me.开始时间 = 开始时间
      Me.耗时 = 耗时
      Me.进度 = 进度
      Me.状态 = 状态
    End Sub

    Public ReadOnly Property 任务名称 As String

    Public ReadOnly Property 开始时间 As String

    Public ReadOnly Property 耗时 As String

    Public ReadOnly Property 进度 As String

    Public ReadOnly Property 状态 As String

  End Class

End Namespace
