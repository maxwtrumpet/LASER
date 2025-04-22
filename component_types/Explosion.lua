Explosion = {
    size = "1",
    explosion = "enemies/explosion/",
    OnStart = function (self)
        local am = self.actor:GetComponent("Animator")
        am.sprite = self.actor:GetComponent("SpriteRenderer")
        am.frames = {self.explosion .. self.size .. "_0", self.explosion .. self.size .. "_1",
                     self.explosion .. self.size .. "_2", self.explosion .. self.size .. "_3",
                     self.explosion .. self.size .. "_4", self.explosion .. self.size .. "_5"}
    end
}