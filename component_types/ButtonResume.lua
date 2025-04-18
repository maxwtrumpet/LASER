ButtonResume = {

    focus = nil,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            Actor.Destroy(Actor.Find("Pause"))
            Actor.Destroy(Actor.Find("ButtonMenu"))
            Actor.Destroy(self.actor)
            Actor.Find("Floor"):GetComponent("GeneralManager").enabled = true
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}