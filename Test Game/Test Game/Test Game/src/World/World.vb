Imports System.Xml
Imports System.IO
Imports System.Text

Public Class World
    'Private standard_tiles As Dictionary(Of Integer, Tile)
    Private tiles(,,) As Tile
    Private collision(,) As Boolean
    Private events As New List(Of WorldEvent)
    Private pathFinder As New PathFinder

    Private width As Integer
    Private height As Integer
    Private depth As Integer

    Private drawColliders As Boolean = True
    Private drawEvents As Boolean = True
    Private noClip As Boolean = False

    Public Sub New(ByVal width As Integer, ByVal height As Integer, ByVal depth As Integer)
        tiles = New Tile(width, height, depth) {}
        collision = New Boolean(width, height) {}
        'Default the world to have no colliders
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
        If Globals.inDebugMode() Then
        End If
    End Sub

    Public Sub update(ByVal gameTime As GameTime)
        updateInput(gameTime)
        noClip = noClip And Globals.inDebugMode() 'If and only if in debug mode keep noClip on

        For Each e As WorldEvent In events
            If e.condition(e.x, e.y) Then
                e.handler(e.x, e.y)
            End If
        Next

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
                    If renderTile IsNot Nothing Then 'If the tile is within world bounds draw it
                        renderTile.render(Globals.spriteBatch, Resources.terrain, i - Globals.player.posX + Player.renderLocation, j - Globals.player.posY + Player.renderLocation)
                    End If
                Next

                If Globals.inDebugMode() AndAlso drawColliders AndAlso getCollision(i, j) Then
                    Dim x As Single = i - Globals.player.posX + Player.renderLocation
                    Dim y As Single = j - Globals.player.posY + Player.renderLocation
                    Globals.spriteBatch.Draw(Resources.debugTextures, New Rectangle(CInt(x * 32.0 * Globals.ZOOM_FACTOR), CInt(y * 32.0 * Globals.ZOOM_FACTOR), 32 * Globals.ZOOM_FACTOR, 32 * Globals.ZOOM_FACTOR), New Rectangle(0, 0, 32, 32), Color.White)
                End If
            Next
        Next

        If Globals.inDebugMode() Then
            Util.drawDebugText("X: " & Globals.player.posX & " Y: " & Globals.player.posY, Color.White)
            If Not drawColliders Then Util.drawDebugText("Not Drawing Colliders", Color.White)
            If noClip Then Util.drawDebugText("No Clip Enabled", Color.White)

            If drawEvents Then
                For Each e As WorldEvent In events
                    Dim x As Single = e.x - Globals.player.posX + Player.renderLocation
                    Dim y As Single = e.y - Globals.player.posY + Player.renderLocation
                    Globals.spriteBatch.Draw(Resources.debugTextures, New Rectangle(CInt(x * 32.0 * Globals.ZOOM_FACTOR), CInt(y * 32.0 * Globals.ZOOM_FACTOR), 32 * Globals.ZOOM_FACTOR, 32 * Globals.ZOOM_FACTOR), New Rectangle(32, 0, 32, 32), Color.White)
                Next
            Else
                Util.drawDebugText("Not Drawing Events", Color.White)
            End If

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
            'I like this next like of code a lot.
            Return Me.collision(x, y) AndAlso Not noClip 'if in noClip mode turn off the collider
        End If
        Return True AndAlso Not noClip 'if in noClip mode turn off the collider
    End Function

    Public Sub addEvent(e As WorldEvent)
        Me.events.Add(e)
    End Sub

    Public Sub addEvents(e As WorldEvent())
        Me.events.AddRange(e)
    End Sub

    Public Function showingEvents() As Boolean
        Return drawEvents
    End Function

    Public Function toggleShowEvents() As Boolean
        drawEvents = Not drawEvents
        Return drawEvents
    End Function

    Public Sub setShowEvents(val As Boolean)
        drawEvents = val
    End Sub

    Public Function showingColliders() As Boolean
        Return drawColliders
    End Function

    Public Function toggleShowColliders() As Boolean
        drawColliders = Not drawColliders
        Return drawColliders
    End Function

    Public Sub setShowColliders(val As Boolean)
        drawColliders = val
    End Sub

    Public Function isNoClipEnabled() As Boolean
        Return noClip
    End Function

    Public Function toggleNoClip() As Boolean
        noClip = Not noClip
        Return noClip
    End Function

    Public Sub setNoClip(val As Boolean)
        noClip = val
    End Sub

    Public Function getWidth() As Integer
        Return width
    End Function

    Public Function getHeight() As Integer
        Return height
    End Function

    Public Function getDepth() As Integer
        Return depth
    End Function

    Public Function getPathFinder() As PathFinder
        Return pathFinder
    End Function

    Public Shared Function loadFromFile(ByRef fileName As String) As World
        Dim mapWidth As Integer
        Dim mapHeight As Integer
        Dim tiles As New List(Of Tile(,))
        Dim eventElements As New List(Of XElement)
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
                            End If
                        Else
                            Dim elements As XElement() = element.Element("data").Elements().ToArray()
                            For i As Integer = 0 To elements.Length() - 1
                                tiles.Item(mapDepth - 1)(i Mod mapWidth, i \ mapHeight) = New Tile(CInt(elements(i).Attribute("gid").Value) - 1)
                            Next
                        End If
                    End If
                Case "events"
                    For Each eventElement As XElement In element.Elements()
                        eventElements.Add(eventElement)
                    Next
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
        If eventElements.Count() > 0 Then
            Dim compiler As New Compiler()
            Dim language As String
            Dim className As String
            Dim location As String
            Dim output As CodeDom.Compiler.CompilerResults
            Dim reader As IO.StreamReader
            For Each eventElement As XElement In eventElements
                className = New String(eventElement.Attribute("className").Value.ToCharArray())
                language = New String(eventElement.Attribute("language").Value.ToCharArray())
                location = New String(eventElement.Attribute("source").Value.ToCharArray())

                compiler.cp.MainClass = className
                compiler.cp.OutputAssembly = className & ".dll"
                reader = New IO.StreamReader(location)

                Select Case language
                    Case "CSharp"
                        output = compiler.compileCS(reader.ReadToEnd())
                    Case "VisualBasic"
                        output = compiler.compileVB(reader.ReadToEnd())
                    Case Else
                        Util.log("Invalid language at event '" & className & "'")
                        Continue For
                End Select
                Util.log("Compiling " & className)
                For Each e As CodeDom.Compiler.CompilerError In output.Errors
                    Util.log(location & "(" & e.Line & "," & e.Column & "): error " & e.ErrorNumber & ": " & e.ErrorText)
                Next

                If output.Errors.HasErrors() Then
                    Util.log("Compilation failed")
                    Continue For
                Else
                    Util.log("Compilation successful")
                End If
                Dim obj As Object = output.CompiledAssembly.CreateInstance(className)
                obj.addEvents(map)
            Next
        End If

        Return map
    End Function

End Class
