ButtonReturn = {

    focus = nil,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            Actor.Instantiate("ButtonLevels")
            Actor.Instantiate("ButtonEndless")
            Actor.Instantiate("ButtonRecords")
            Actor.Instantiate("ButtonQuit")
            Actor.Instantiate("ButtonControls")
            Actor.Instantiate("ButtonCredits")
            Actor.Instantiate("Logo")
            local existing_buttons = Actor.FindAll("ButtonLevel")
            for index, value in ipairs(existing_buttons) do
                Actor.Destroy(value)
            end
            Actor.Destroy(self.actor)
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}