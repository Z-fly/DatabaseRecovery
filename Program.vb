Imports Avalonia
Imports Avalonia.ReactiveUI

NotInheritable Class Program
  
  <STAThread>
  Public Shared Sub Main(args As String())
    Call BuildAvaloniaApp.StartWithClassicDesktopLifetime(args)
  End Sub

  ' Avalonia configuration, don't remove; also used by visual designer.
  Public Shared Function BuildAvaloniaApp() As AppBuilder
    Return AppBuilder.Configure(Of App)().UsePlatformDetect.LogToTrace.WithInterFont.UseReactiveUI()

  End Function

End Class