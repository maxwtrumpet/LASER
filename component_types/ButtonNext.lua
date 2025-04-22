ButtonNext = {

    focus = nil,
    level = 0,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            Scene.Load("Level" .. tostring(self.level + 1))
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}