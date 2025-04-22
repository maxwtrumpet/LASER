ButtonResume = {

    focus = nil,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            Actor.Destroy(Actor.Find("GameMenu"))
            Actor.Destroy(Actor.Find("ButtonMenu"))
            Actor.Destroy(self.actor)
            Actor.Find("Floor"):GetComponent("GeneralManager").enabled = true
            Actor.Find("UI"):GetComponentByKey("Manager").enabled = true
            Actor.Find("Gun"):EnableAll()
            Actor.Find("Player"):EnableAll()
            local beam = Actor.Find("Beam")
            if beam ~= nil then
                beam:EnableAll()
            end
            local basic = Actor.FindAll("Basic")
            for index, value in ipairs(basic) do
                value:EnableAll()
                value:GetComponentByKey("XManager"):OnEnable()
            end
            local fast = Actor.FindAll("Fast")
            for index, value in ipairs(fast) do
                value:EnableAll()
            end
            local egg = Actor.FindAll("Egg")
            for index, value in ipairs(egg) do
                value:EnableAll()
            end
            local gnat = Actor.FindAll("Gnat")
            for index, value in ipairs(gnat) do
                value:EnableAll()
                value:GetComponentByKey("XManager"):OnEnable()
            end
            local kamikaze = Actor.FindAll("Kamikaze")
            for index, value in ipairs(kamikaze) do
                value:EnableAll()
                value:GetComponentByKey("XManager"):OnEnable()
            end
            local smoke = Actor.FindAll("Smoke")
            for index, value in ipairs(smoke) do
                value:EnableAll()
            end
            local boss = Actor.FindAll("Boss")
            for index, value in ipairs(boss) do
                value:EnableAll()
                value:GetComponentByKey("XManager"):OnEnable()
            end
            local explosions = Actor.FindAll("Explosion")
            for index, value in ipairs(explosions) do
                value:EnableAll()
            end
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}