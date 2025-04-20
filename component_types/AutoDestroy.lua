AutoDestroy = {
    time = 60,
    total = 0,

    OnUpdate = function (self)
        self.total = self.total + 1
        if self.total == self.time then
            Actor.Destroy(self.actor)
        end
    end
}