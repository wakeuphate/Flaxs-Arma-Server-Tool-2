﻿
Public Class Setup
    'Checks to see if setup has been run before - continues to main window if true
    Private Sub Setup_Initialized(sender As Object, e As EventArgs) Handles MyBase.Initialized
        If My.Settings.firstRun = False Then
            Dim main = New MainWindow
            main.Show()
            Close()
        End If
    End Sub

    'Makes close button red when mouse is over button
    Private Sub WindowCloseButton_MouseEnter(sender As Object, e As MouseEventArgs) Handles IWindowCloseButton.MouseEnter
        Dim converter = New BrushConverter()
        Dim brush = CType(converter.ConvertFromString("#D72C2C"), Brush)

        IWindowCloseButton.Background = brush
    End Sub

    'Changes colour of close button back to UI base when mouse leaves button
    Private Sub WindowCloseButton_MouseLeave(sender As Object, e As MouseEventArgs) Handles IWindowCloseButton.MouseLeave
        Dim brush = FindResource("MaterialDesignPaper")

        IWindowCloseButton.Background = brush
    End Sub

    'Closes app when using custom close button
    Private Sub WindowCloseButton_Selected(sender As Object, e As RoutedEventArgs) Handles IWindowCloseButton.Selected
        Close()
    End Sub

    'Minimises app when using custom minimise button
    Private Sub WindowMinimizeButton_Selected(sender As Object, e As RoutedEventArgs) Handles IWindowMinimizeButton.Selected
        IWindowMinimizeButton.IsSelected = False
        WindowState = WindowState.Minimized
    End Sub

    'Allows user to move the window around using the custom nav bar
    Private Sub WindowDragBar_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles IWindowDragBar.MouseLeftButtonDown, ILogoImage.MouseLeftButtonDown, IWindowTitle.MouseLeftButtonDown
        DragMove()
    End Sub

    'Opens folder select dialog when clicking certain buttons
    Private Sub DirButton_Click(sender As Object, e As RoutedEventArgs) Handles ISteamDirButton.Click, IServerDirButton.Click
        Dim path As String = MainWindow.SelectFolder()

        If path IsNot Nothing Then
            If sender Is ISteamDirButton Then
                ISteamDirBox.Text = path
            ElseIf sender Is IServerDirButton Then
                IServerDirBox.Text = path
            End If
        End If
    End Sub

    'Contiues to main form when button is clicked - stores users options in settings and encrypts steam password
    Private Sub IContinueButton_Click(sender As Object, e As RoutedEventArgs) Handles IContinueButton.Click
        My.Settings.serverPath = IServerDirBox.Text
        My.Settings.steamCMDPath = ISteamDirBox.Text
        My.Settings.steamUserName = ISteamUserBox.Text
        My.Settings.firstRun = False

        Dim encryptionString As String = Environment.UserName & Encryption.SystemSerialNumber()
        Dim wrapper As New Encryption(encryptionString)
        Dim cypher As String = wrapper.EncryptData(ISteamPassBox.Password)
        My.Settings.steamPassword = cypher

        Dim main = New MainWindow With {
            .InstallSteamCmd = True
        }
        main.Show()
        Close()
    End Sub
End Class