Public Class CommandLine

    Public Delegate Sub CommandHandler(parameters As Object())

    Private open As Boolean = False
    Private currentCommand As String = ""
    Private history As New List(Of String)
    Private commands As New Dictionary(Of String, Tuple(Of String, CommandHandler))

    Public Sub New()
        Me.addCommand("clear", Sub(arguments As Object()) history.Clear())
        addCommand("help", AddressOf help)
        addCommand("?", AddressOf help)
        addCommand("say", "si", AddressOf say)
    End Sub

    Public Sub update(ByVal gameTime As GameTime)
        If Input.keyPressed(Input.COMMAND_LINE_KEY) And Not open Then
            open = Not open
            Globals.gamePaused = True
        ElseIf Input.keyPressed(Keys.Escape) And open Then
            open = False
            currentCommand = ""
            Globals.gamePaused = False
        ElseIf open Then
            For Each key As Keys In Input.getPressedKeys()
                If Input.keyPressed(key) Then
                    Select Case key
                        Case Keys.Enter
                            execute(currentCommand)
                            currentCommand = ""
                            'open = False
                        Case Keys.Back
                            If currentCommand.Length() > 0 Then currentCommand = currentCommand.Substring(0, currentCommand.Length() - 1)
                        Case Else
                            currentCommand = currentCommand & keyToChar(key, Input.isKeyDown(Keys.LeftShift) OrElse Input.isKeyDown(Keys.RightShift))
                    End Select
                End If
            Next
        End If
    End Sub

    Public Sub draw(ByVal gameTime As GameTime)
        If open Then
            Dim stringsDrawn As Integer = 0
            If currentCommand.Length() <> 0 Then
                Globals.spriteBatch.DrawString(Resources.georgia_16, "> " & currentCommand, New Vector2(0, 11 * 64 - 25), Color.White)
            Else
                Globals.spriteBatch.DrawString(Resources.georgia_16, "> ", New Vector2(0, 11 * 64 - 25), Color.White)
            End If
            history.Reverse()
            For Each s As String In history
                stringsDrawn += 1
                Globals.spriteBatch.DrawString(Resources.georgia_16, s, New Vector2(0, 11 * 64 - 25 - (25 * stringsDrawn)), Color.White)
            Next
            history.Reverse()
        End If
    End Sub

    Public Sub execute(command As String)
        Dim tokens As New List(Of String)
        Dim workingCommand As String = command
        While workingCommand.Length() > 0
            Dim result As Tuple(Of String, String) = getToken(workingCommand)
            workingCommand = result.Item2
            tokens.Add(result.Item1)
        End While

        If tokens.Count = 0 Then
            Return
        End If

        Dim commandName As String = tokens(0)
        tokens.RemoveRange(0, 1)
        If Not commands.ContainsKey(commandName) Then
            print("Error: No such command '" & commandName & "'")
        Else
            Dim argumentList As New List(Of Object)
            Dim syntax As String = commands(commandName).Item1
            If tokens.Count <> syntax.Length() Then
                print("Error: Invalid number of parameters!")
                printCommandSyntax(commandName)
                Return
            End If
            For i As Integer = 0 To syntax.Length() - 1
                Select Case syntax(i)
                    Case "i"c
                        Dim output As Integer
                        If Not Integer.TryParse(tokens(i), output) Then
                            print("Error: Invalid argument(s) provided!")
                            printCommandSyntax(commandName)
                            Return
                        Else
                            argumentList.Add(output)
                        End If
                    Case "f"c
                        Dim output As Single
                        If Not Single.TryParse(tokens(i), output) Then
                            print("Error: Invalid argument(s) provided!")
                            printCommandSyntax(commandName)
                            Return
                        Else
                            argumentList.Add(output)
                        End If
                    Case "c"c
                        If tokens(i).Length() > 1 Then
                            print("Error: Invalid argument(s) provided!")
                            printCommandSyntax(commandName)
                            Return
                        Else
                            argumentList.Add(tokens(i)(0))
                        End If
                    Case "b"c
                        Dim output As Boolean
                        If Not Boolean.TryParse(tokens(i), output) Then
                            print("Error: Invalid argument(s) provided!")
                            printCommandSyntax(commandName)
                            Return
                        Else
                            argumentList.Add(output)
                        End If
                    Case Else
                        argumentList.Add(tokens(i))
                End Select
            Next
            commands(commandName).Item2.Invoke(argumentList.ToArray())
        End If
    End Sub

    Private Function getToken(command As String) As Tuple(Of String, String)
        Dim token As String = ""
        Dim inString As Boolean = False
        Dim escaped As Boolean = False
        Dim c As Char
        Dim i As Integer
        For i = 0 To command.Length() - 1
            c = command(i)
            Select Case c
                Case " "c
                    If Not inString Then
                        If token.Length() = 0 Then Continue For Else Exit For
                    Else
                        token = token & c
                    End If
                Case """"c
                    If Not inString Then
                        inString = True
                        escaped = False
                    Else
                        If Not escaped Then
                            inString = False
                            Exit For
                        Else
                            escaped = False
                            token = token & c
                        End If
                    End If
                Case "\"c
                    If Not escaped Then
                        escaped = True
                    Else
                        escaped = False
                        token = token & c
                    End If
                Case Else
                    token = token & c
            End Select
        Next
        Return New Tuple(Of String, String)(token, command.Remove(0, Math.Min(i + 1, command.Length)))
    End Function

    Public Sub addCommand(commandName As String, argumentList As String, handler As CommandHandler)
        commands.Add(commandName, New Tuple(Of String, CommandHandler)(argumentList, handler))
    End Sub

    Public Sub addCommand(commandName As String, handler As CommandHandler)
        addCommand(commandName, "", handler)
    End Sub

    Public Sub print(text As String)
        history.Add(text)
    End Sub

    Private Sub printCommandSyntax(command As String)
        Dim syntax As String = commands(command).Item1
        Dim output As String = command
        For Each c As Char In syntax
            Select Case c
                Case "i"c
                    output = output & " <Int>"
                Case "f"c
                    output = output & " <Float>"
                Case "s"c
                    output = output & " <String>"
                Case "c"c
                    output = output & " <Char>"
                Case "b"c
                    output = output & " <Boolean>"
            End Select
        Next
        print(output)
    End Sub

    Private Function keyToChar(key As Keys, shift As Boolean) As String
        If key >= Keys.A AndAlso key <= Keys.Z Then
            If shift Then
                Return key.ToString()
            Else
                Return key.ToString().ToLower()
            End If
        ElseIf key >= Keys.NumPad0 AndAlso key <= Keys.NumPad9 Then
            Return CInt(key - Keys.NumPad0).ToString()
        ElseIf key >= Keys.D0 AndAlso key <= Keys.D9 Then
            Dim val As Integer = CInt(key - Keys.D0)
            If Not shift Then
                Return val.ToString()
            Else
                Select Case val
                    Case 1
                        Return "!"
                    Case 2
                        Return "@"
                    Case 3
                        Return "#"
                    Case 4
                        Return "$"
                    Case 5
                        Return "%"
                    Case 6
                        Return "^"
                    Case 7
                        Return "&"
                    Case 8
                        Return "*"
                    Case 9
                        Return "("
                    Case 0
                        Return ")"
                End Select
            End If
        Else
            Select Case key
                Case Keys.Space
                    Return " "
                Case Keys.OemMinus
                    If shift Then Return "_" Else  : Return "-"
                Case Keys.OemPlus
                    If shift Then Return "+" Else  : Return "="
                Case Keys.OemOpenBrackets
                    If shift Then Return "{" Else  : Return "["
                Case Keys.OemCloseBrackets
                    If shift Then Return "}" Else  : Return "]"
                Case Keys.OemSemicolon
                    If shift Then Return ":" Else  : Return ";"
                Case Keys.OemQuotes
                    If shift Then Return """" Else  : Return "'"
                Case Keys.OemComma
                    If shift Then Return "<" Else  : Return ","
                Case Keys.OemPeriod
                    If shift Then Return ">" Else  : Return "."
                Case Keys.OemQuestion
                    If shift Then Return "?" Else  : Return "/"
                Case Keys.OemPipe
                    If shift Then Return "|" Else  : Return "\"
                Case Keys.OemTilde
                    If shift Then Return "~" Else  : Return "`"
            End Select
        End If
        Return ""
    End Function

    Private Sub help(arguments As Object())
        print("Commands:")
        For Each key As String In commands.Keys
            printCommandSyntax(key)
        Next
    End Sub

    Private Sub say(arguments As Object())
        For i As Integer = 1 To CInt(arguments(1))
            print(CStr(arguments(0)))
        Next
    End Sub

End Class
