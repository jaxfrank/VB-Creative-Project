Public MustInherit Class Screen

    Public MustOverride Sub init() 'Called every time this screen is made the current screen

    Public MustOverride Sub update(gameTime As GameTime)

    Public MustOverride Sub render(gameTime As GameTime)

End Class
