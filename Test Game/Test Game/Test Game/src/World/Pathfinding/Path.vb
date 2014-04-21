Public Class Path

    Private nodes As Point()
    Private currentNode As Integer = 0

    Public Sub New(nodeList As Point())
        nodes = DirectCast(nodeList.Clone(), Point())
    End Sub

    Public Function nextNode() As Point
        Dim current As Integer = currentNode
        currentNode += 1
        Return nodes(current)
    End Function

    Public Function previousNode() As Point
        Dim current As Integer = currentNode
        currentNode -= 1
        Return nodes(current)
    End Function

    Public Sub first()
        currentNode = 0
    End Sub

    Public Sub last()
        currentNode = nodes.Length() - 1
    End Sub

    Public Sub gotoNode(nodeIndex As Integer)
        If nodeIndex >= nodes.Length() Or nodeIndex < 0 Then
            currentNode = 0
            Return
        End If
        currentNode = nodeIndex
    End Sub

    Public Function getNodes() As Point()
        Return nodes
    End Function

    Public Function length() As Integer
        Return nodes.Length()
    End Function

End Class
