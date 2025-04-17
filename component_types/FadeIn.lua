FadeIn = {
    OnStart = function (self)
        self.sr = self.actor:GetComponent("SpriteRenderer")
        self.alpha = 255
    end,
    OnUpdate = function(self)
        self.alpha = self.alpha - 2.55
        if self.alpha <= 0 then Actor.Destroy(self.actor)
        else self.sr.a = self.alpha
        end
    end
}