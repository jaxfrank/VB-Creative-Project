Public Class PathNodeComparer
    Implements IComparer(Of PathNode)

    Public Function Compare(x As PathNode, y As PathNode) As Integer Implements System.Collections.Generic.IComparer(Of PathNode).Compare
        Return x.f_totalCost.CompareTo(y.f_totalCost)
    End Function
End Class
