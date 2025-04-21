GeneralManager = {

    OnUpdate = function (self)
        Application.HideCursor()
        Input.SetMousePosition(Vector2(0,0))
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
            local egg = Actor.FindAll("Egg")
            for index, value in ipairs(egg) do
                value:DisableAll()
            end
            local gnat = Actor.FindAll("Gnat")
            for index, value in ipairs(gnat) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local kamikaze = Actor.FindAll("Kamikaze")
            for index, value in ipairs(kamikaze) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local smoke = Actor.FindAll("Smoke")
            for index, value in ipairs(smoke) do
                value:DisableAll()
            end
            local boss = Actor.FindAll("Boss")
            for index, value in ipairs(boss) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local explosions = Actor.FindAll("Explosion")
            for index, value in ipairs(explosions) do
                value:DisableAll()
            end
            self.enabled = false
        end
    end

}