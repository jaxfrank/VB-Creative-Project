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
        Return False
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

    Public Function getWidth() As Integer
        Return width
    End Function

    Public Function getHeight() As Integer
        Return height
    End Function

    Public Function getDepth() As Integer
        Return depth
    End Function

End Class
