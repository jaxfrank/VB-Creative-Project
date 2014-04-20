Public Class WorldEvent

    Public Delegate Function EventCondition(x As Integer, y As Integer) As Boolean
    Public Delegate Sub EventHandler(x As Integer, y As Integer)

    Public Sub New(x As Integer, y As Integer, condition As EventCondition, handler As EventHandler)
        Me.x = x
        Me.y = y
        Me.condition = condition
        Me.handler = handler
    End Sub

    'Position of the tile on which this event is going to occur
    Public x As Integer
    Public y As Integer
    'The function which determines if the handler will be called
    Public condition As EventCondition
    'The function that is called when the event actually happens
    Public handler As EventHandler

End Class
