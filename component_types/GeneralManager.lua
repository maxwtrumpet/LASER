GeneralManager = {

    OnUpdate = function (self)
        if Input.IsKeyJustDown("escape") then
            Event.Publish("ButtonPress", {})
            Actor.Instantiate("ButtonMenu")
            Actor.Instantiate("ButtonResume")
            Actor.Instantiate("Pause"):GetComponent("ButtonManager").button_layout = {2}
            self.enabled = false
        end
    end

}