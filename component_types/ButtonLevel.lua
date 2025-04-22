ButtonLevel = {

    focus = nil,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            local queued_level = self.actor:GetComponent("TextRenderer").text
            Scene.Load("Level" .. queued_level)
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}