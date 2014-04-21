Public Class PathNode

    Public Property parent As PathNode = Nothing
    Public Property mapPosition As Point
    Public Property h_hueristic As Integer
    Public Property g_movementCost As Integer = 1
    Public Property f_totalCost As Integer

    Public Sub calcF()
        f_totalCost = h_hueristic + g_movementCost
    End Sub

End Class
