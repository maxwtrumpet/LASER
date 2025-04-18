EndlessManager = {
    OnStart = function (self)
        self.actor:RemoveComponent(self.actor:GetComponentByKey("EM"))
        self.actor:RemoveComponent(self.actor:GetComponentByKey("Meter"))
        self.actor:RemoveComponent(self.actor:GetComponentByKey("Backdrop"))
        self.actor:RemoveComponent(self.actor:GetComponentByKey("Progress"))
        local mm = Actor.Find("StaticData")
        if mm == nil then
            mm = Actor.Instantiate("StaticData")
            Actor.DontDestroy(mm)
            mm:GetComponent("MusicManager"):Init()
        end
        Event.Publish("Level", {level = 10})
    end
}