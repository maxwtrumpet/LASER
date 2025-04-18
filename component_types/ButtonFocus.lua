ButtonFocus = {

    row = 0,
    column = 0,

    _OnFocus = function (self, event)
        if event.row == self.row and event.column == self.column then
            self.selected.enabled = event.focus
        end
    end,

    OnStart = function (self)
        self.selected = self.actor:GetComponentByKey("Selected")
        Event.Subscribe("ButtonFocus", self, self._OnFocus)
    end

}