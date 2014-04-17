Imports System.Xml
Imports System.IO
Imports System.Text

Public Class World
    Private standard_tiles As Dictionary(Of Integer, Tile)
    Private tiles(,,) As Tile
    Private collision(,) As Boolean
    Private width As Integer
    Private height As Integer
    Private depth As Integer

    Private Shared debugKey As Keys = Keys.F1
    Private debugMode As Boolean = False
    Private wasDebugKeyPressed As Boolean = False

    Public Sub New(ByVal width As Integer, ByVal height As Integer, ByVal depth As Integer)
        tiles = New Tile(width, height, depth) {}
        collision = New Boolean(width, height) {}
        For i As Integer = 0 To width
            For j As Integer = 0 To height
                collision(i, j) = False
            Next
        Next
        Me.width = width
        Me.height = height
        Me.depth = depth
    End Sub

    Private Sub updateInput(ByVal gameTime As GameTime)
        If Input.isKeyDown(debugKey) AndAlso Not wasDebugKeyPressed Then
            debugMode = Not debugMode
            wasDebugKeyPressed = True
        ElseIf Not Input.isKeyDown(debugKey) AndAlso wasDebugKeyPressed Then
            wasDebugKeyPressed = False
        End If
    End Sub

    Public Sub update(ByVal gameTime As GameTime)
        updateInput(gameTime)
    End Sub

    Public Sub draw(ByVal gameTime As GameTime)
        For i = Globals.player.posX - 10 To Globals.player.posX + 11
            For j = Globals.player.posY - 10 To Globals.player.posY + 11
                For k = 0 To getDepth() - 1
                    Dim renderTile As Tile = getTile(i, j, k)
                    'I hate VB so much
                    'why have so many different ways of saying != and || and &&
                    'also this is a double negative and since VB already has so many key words why don't
                    'they just go ahead and make something(or better yet aThing) a keyword so you can say
                    '
                    'if VB is Something or VB is aThing then 
                    '   commitSuicide()
                    'end if
                    If renderTile IsNot Nothing Then
                        renderTile.render(Globals.spriteBatch, Resources.terrain, i - Globals.player.posX + Player.renderLocation, j - Globals.player.posY + Player.renderLocation)
                    End If
                Next

                If debugMode AndAlso getCollision(i, j) Then
                    Dim x As Single = i - Globals.player.posX + Player.renderLocation
                    Dim y As Single = j - Globals.player.posY + Player.renderLocation
                    Globals.spriteBatch.Draw(Resources.collisionDebugTexture, New Rectangle(CInt(x * 32.0 * Globals.ZOOM_FACTOR), CInt(y * 32.0 * Globals.ZOOM_FACTOR), 32 * Globals.ZOOM_FACTOR, 32 * Globals.ZOOM_FACTOR), New Rectangle(0, 0, 32, 32), Color.White)
                End If
            Next
        Next

        If debugMode Then
            Globals.spriteBatch.DrawString(Resources.georgia_16, "X: " & Globals.player.posX & " Y: " & Globals.player.posY, New Vector2(0, 0), Color.White)
        End If
    End Sub

    Public Function setTile(ByVal x As Integer, ByVal y As Integer, ByVal depth As Integer, ByRef tile As Tile) As Boolean
        If x >= 0 AndAlso x < tiles.GetLength(0) AndAlso y >= 0 AndAlso y < tiles.GetLength(1) AndAlso depth >= 0 AndAlso depth < tiles.GetLength(2) Then
            tiles(x, y, depth) = tile
            Return True
        End If
        Return False
    End Function

    Public Function getTile(ByVal x As Integer, ByVal y As Integer, ByVal depth As Integer) As Tile
        If x < 0 OrElse x > Me.width - 1 OrElse y < 0 OrElse y > Me.height - 1 OrElse depth < 0 OrElse depth > Me.depth - 1 Then
            Return Nothing
        End If
        Return tiles(x, y, depth)
    End Function

    Public Sub setCollision(ByVal x As Integer, ByVal y As Integer, ByVal collision As Boolean)
        If x >= 0 AndAlso x < Me.width AndAlso y >= 0 AndAlso y < Me.height Then
            Me.collision(x, y) = collision
        End If
    End Sub

    Public Sub setCollision(ByVal collisions As Boolean(,))
        Me.collision = collisions
    End Sub

    Public Function getCollision(ByVal x As Integer, ByVal y As Integer) As Boolean
        If x >= 0 AndAlso x < Me.width AndAlso y >= 0 AndAlso y < Me.height Then
            Return Me.collision(x, y)
        End If
        Return True
    End Function

    Public Function getWidth() As Integer
        Return width
    End Function

    Public Function getHeight() As Integer
        Return height
    End Function

    Public Function getDepth() As Integer
        Return depth
    End Function

    Public Shared Function loadFromFile(ByRef fileName As String) As World
        Dim mapWidth As Integer
        Dim mapHeight As Integer
        Dim tiles As New List(Of Tile(,))
        Dim collisionLayer As Boolean(,) = Nothing
        Dim document As XDocument = XDocument.Load(Globals.content.RootDirectory & "/" & fileName & ".tmx")
        Dim mapElement As XElement = document.Element("map")
        Dim mapDepth As Integer = 0
        For Each attribute As XAttribute In mapElement.Attributes
            Select Case attribute.Name
                Case "width"
                    mapWidth = CInt(attribute.Value)
                Case "height"
                    mapHeight = CInt(attribute.Value)
            End Select
        Next
        For Each element As XElement In mapElement.Elements()
            Select Case element.Name
                Case "layer"
                    If element.Attribute("name").Value = "collision" Then
                        collisionLayer = New Boolean(mapWidth, mapHeight) {}
                        If element.Element("data").HasAttributes() Then
                            'If element.Element("data").Attribute("encoding").Value = "csv" Then
                            Dim gids As String() = element.Element("data").Value.Split(","c)
                            For i As Integer = 0 To mapWidth * mapHeight - 1
                                collisionLayer(i Mod mapWidth, i \ mapHeight) = CInt(gids(i)) > 0
                            Next
                            'End If
                        Else
                            Dim elements As XElement() = element.Element("data").Elements().ToArray()
                            For i As Integer = 0 To elements.Length() - 1
                                collisionLayer(i Mod mapWidth, i \ mapHeight) = CInt(elements(i).Attribute("gid").Value) > 0
                            Next
                        End If
                    Else
                        mapDepth = mapDepth + 1
                        tiles.Add(New Tile(mapWidth, mapHeight) {})
                        If element.Element("data").HasAttributes() Then
                            If element.Element("data").Attribute("encoding").Value = "csv" Then
                                Dim gids As String() = element.Element("data").Value.Split(","c)
                                For i As Integer = 0 To mapWidth * mapHeight - 1
                                    tiles.Item(mapDepth - 1)(i Mod mapWidth, i \ mapHeight) = New Tile(CInt(gids(i)) - 1)
                                Next
                            Else
                                'Code removed because it does not work
                                'This code is here to handle Base64 data format
                                '
                                'Dim byteArray As Byte() = Convert.FromBase64String(element.Value)

                                '' const int gid = data[i] |
                                ''    data[i + 1] << 8 |
                                ''    data[i + 2] << 16 |
                                ''     data[i + 3] << 24;
                                'Dim textFile As StreamWriter = File.CreateText("testOutput" & mapDepth & ".txt")
                                'Dim currentLine As Integer = 0
                                'For i As Integer = 0 To mapWidth * mapHeight - 1
                                '    Dim gid As Long = (byteArray(i * 4) Or (byteArray(i * 4 + 1) << 8) Or (byteArray(i * 4 + 2) << 16) Or (byteArray(i * 4 + 3) << 24)) - 1
                                '    tiles.Item(mapDepth - 1)(i Mod mapWidth, i \ mapHeight) = New Tile(CInt(gid))
                                '    If i \ mapHeight > currentLine Then
                                '        currentLine += 1
                                '        textFile.Write(vbNewLine)
                                '    End If
                                '    textFile.Write(gid)
                                '    textFile.Write(",")
                                'Next
                            End If
                        Else
                            Dim elements As XElement() = element.Element("data").Elements().ToArray()
                            For i As Integer = 0 To elements.Length() - 1
                                tiles.Item(mapDepth - 1)(i Mod mapWidth, i \ mapHeight) = New Tile(CInt(elements(i).Attribute("gid").Value) - 1)
                            Next
                        End If
                    End If
            End Select
        Next
        Dim map As World = New World(mapWidth, mapHeight, mapDepth)
        For i As Integer = 0 To mapWidth - 1
            For j As Integer = 0 To mapHeight - 1
                For k As Integer = 0 To mapDepth - 1
                    map.setTile(i, j, k, tiles.Item(k)(i, j))
                Next
            Next
        Next
        If collisionLayer IsNot Nothing Then
            map.setCollision(collisionLayer)
        End If
        Return map
    End Function

End Class
