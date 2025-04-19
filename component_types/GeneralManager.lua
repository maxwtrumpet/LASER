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
            self.enabled = false
        end
    end

}