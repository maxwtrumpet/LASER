GeneralManager = {

    OnUpdate = function (self)
        if Input.IsKeyJustDown("escape") then
            Event.Publish("ButtonPress", {})
            Actor.Instantiate("ButtonMenu")
            Actor.Instantiate("ButtonResume")
            local pause = Actor.Instantiate("GameMenu")
            pause:GetComponent("ButtonManager").button_layout = {2}
            pause:GetComponent("TextRenderer").text = "Pause"
            Actor.Find("UI"):GetComponentByKey("Manager").enabled = false
            Actor.Find("Gun"):DisableAll()
            Actor.Find("Player"):DisableAll()
            local beam = Actor.Find("Beam")
            if beam ~= nil then
                beam:DisableAll()
            end
            local basic = Actor.FindAll("Basic")
            for index, value in ipairs(basic) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local fast = Actor.FindAll("Fast")
            for index, value in ipairs(fast) do
                value:DisableAll()
            end
            local explosions = Actor.FindAll("Explosion")
            for index, value in ipairs(explosions) do
                value:DisableAll()
            end
            self.enabled = false
        end
    end

}