ButtonToInfo = {

    menu = "???",
    focus = nil,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            local back = Actor.Instantiate(self.menu):GetComponent("ButtonBack")
            back.row = self.focus.row
            back.column = self.focus.column
            Actor.Destroy(Actor.Find("Logo"))
            Actor.Destroy(Actor.Find("ButtonLevels"))
            Actor.Destroy(Actor.Find("ButtonEndless"))
            Actor.Destroy(Actor.Find("ButtonRecords"))
            Actor.Destroy(Actor.Find("ButtonQuit"))
            Actor.Destroy(Actor.Find("ButtonControls"))
            Actor.Destroy(Actor.Find("ButtonCredits"))
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}