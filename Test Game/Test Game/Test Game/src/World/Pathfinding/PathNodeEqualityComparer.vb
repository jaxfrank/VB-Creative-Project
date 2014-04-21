Public Class PathNodeEqualityComparer
    Implements IEqualityComparer(Of PathNode)

    Public Function Equals1(x As PathNode, y As PathNode) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of PathNode).Equals
        Return x.mapPosition.X = y.mapPosition.X And x.mapPosition.Y = y.mapPosition.Y
    End Function

    Public Function GetHashCode1(obj As PathNode) As Integer Implements System.Collections.Generic.IEqualityComparer(Of PathNode).GetHashCode
        Return obj.mapPosition.GetHashCode()
    End Function
End Class
