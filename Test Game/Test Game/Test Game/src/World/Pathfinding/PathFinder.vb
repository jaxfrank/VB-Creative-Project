Imports System.Collections.Generic

Public Class PathFinder

    Dim pathNodeComparer As New PathNodeComparer()
    Dim pathNodeEqualityComparer As New PathNodeEqualityComparer()
    Dim standardMovementCost As Integer = 1
    Dim expectedMaxPathLength As Integer = 100
    Dim startPoint As Point
    Dim endPoint As Point

    Public Function findPath(ByRef world As World, startPoint As Point, endPoint As Point) As Path
        Me.startPoint = startPoint
        Dim openList As New List(Of PathNode)
        Dim closedList As New List(Of PathNode)

        Dim beginningNode As New PathNode
        beginningNode.mapPosition = startPoint
        beginningNode.h_hueristic = calculateHeuristic(startPoint, endPoint)
        beginningNode.g_movementCost = 0
        beginningNode.calcF()
        beginningNode.parent = Nothing
        openList.Add(beginningNode)

        Dim endingPathNode As New PathNode
        endingPathNode.mapPosition = endPoint
        endingPathNode.h_hueristic = 0
        endingPathNode.g_movementCost = standardMovementCost
        beginningNode.calcF()

        While Not closedList.Contains(endingPathNode, pathNodeEqualityComparer)
            If openList.Count() = 0 Then Return Nothing 'No path to end point exists

            openList.Sort(pathNodeComparer)
            Dim currentNode As PathNode = openList(0)
            openList.RemoveAt(0)
            closedList.Add(currentNode)

            Dim adjacentNodes As PathNode() = getAdjacentNodes(world, currentNode.mapPosition, endPoint)
            For Each adjacent As PathNode In adjacentNodes
                If Not closedList.Contains(adjacent, pathNodeEqualityComparer) Then
                    If Not openList.Contains(adjacent, pathNodeEqualityComparer) Then
                        adjacent.parent = currentNode
                        adjacent.g_movementCost = currentNode.g_movementCost + adjacent.g_movementCost
                        adjacent.calcF()
                        openList.Add(adjacent)
                    Else
                        For Each node In openList
                            If pathNodeEqualityComparer.Equals1(node, adjacent) Then
                                adjacent = node
                                Exit For
                            End If
                        Next
                        If currentNode.g_movementCost > adjacent.g_movementCost + standardMovementCost Then
                            currentNode.parent = adjacent
                            currentNode.g_movementCost = adjacent.g_movementCost + currentNode.g_movementCost
                            currentNode.calcF()
                            'openList.Remove(adjacent)
                            'openList.Add(adjacent)
                        End If
                    End If
                End If
            Next

        End While
        Dim pathList As New List(Of Point)
        closedList.Sort(pathNodeComparer)
        Dim currentPathNode As PathNode = Nothing
        For Each node As PathNode In closedList
            If pathNodeEqualityComparer.Equals1(node, endingPathNode) Then
                currentPathNode = node
                Exit For
            End If
        Next
        If currentPathNode Is Nothing Then Return Nothing

        While currentPathNode.parent IsNot Nothing
            pathList.Add(currentPathNode.mapPosition)
            currentPathNode = currentPathNode.parent
        End While
        pathList.Add(currentPathNode.mapPosition)
        pathList.Reverse()
        Return New Path(pathList.ToArray())
    End Function

    Private Function getAdjacentNodes(ByRef world As World, position As Point, endPoint As Point) As PathNode()
        Dim nodes As New List(Of PathNode)
        If Not world.getCollision(position.X - 1, position.Y) Then
            Dim location As New Point(position.X - 1, position.Y)
            nodes.Add(New PathNode With {
                      .h_hueristic = calculateHeuristic(location, endPoint),
                      .g_movementCost = standardMovementCost,
                      .f_totalCost = .g_movementCost + .h_hueristic,
                      .mapPosition = location})
        End If
        If Not world.getCollision(position.X + 1, position.Y) Then
            Dim location As New Point(position.X + 1, position.Y)
            nodes.Add(New PathNode With {
                      .h_hueristic = calculateHeuristic(location, endPoint),
                      .g_movementCost = standardMovementCost,
                      .f_totalCost = .g_movementCost + .h_hueristic,
                      .mapPosition = location})
        End If
        If Not world.getCollision(position.X, position.Y - 1) Then
            Dim location As New Point(position.X, position.Y - 1)
            nodes.Add(New PathNode With {
                      .h_hueristic = calculateHeuristic(location, endPoint),
                      .g_movementCost = standardMovementCost,
                      .f_totalCost = .g_movementCost + .h_hueristic,
                      .mapPosition = location})
        End If
        If Not world.getCollision(position.X, position.Y + 1) Then
            Dim location As New Point(position.X, position.Y + 1)
            nodes.Add(New PathNode With {
                      .h_hueristic = calculateHeuristic(location, endPoint),
                      .g_movementCost = standardMovementCost,
                      .f_totalCost = .g_movementCost + .h_hueristic,
                      .mapPosition = location})
        End If
        Return nodes.ToArray()
    End Function

    'manhattan heuristic
    Protected Function calculateHeuristic(point As Point, endPoint As Point) As Integer
        'Dim dx1 = point.X - endPoint.X
        'Dim dy1 = point.Y - endPoint.Y
        'Dim dx2 = startPoint.X - endPoint.X
        'Dim dy2 = startPoint.Y - endPoint.Y
        'Dim cross = Math.Abs(dx1 * dy2 - dx2 * dy1)
        'Return CInt(standardMovementCost * (Math.Abs(point.X - endPoint.X) + Math.Abs(point.Y - endPoint.Y)) + (cross))
        Return CInt(standardMovementCost * (Math.Abs(point.X - endPoint.X) + Math.Abs(point.Y - endPoint.Y)) * (1.0 + 1.0 / expectedMaxPathLength))
        'Return CInt(Math.Pow((Math.Abs(endPoint.X - point.X) + Math.Abs(endPoint.Y - point.Y)), 2))
    End Function

End Class
