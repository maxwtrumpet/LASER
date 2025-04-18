ButtonManager = {
    screen = "???",
    cur_row = 1,
    cur_column = 1,
    button_layout = {},

    OnStart = function (self)
        self.button_layout = Actor.Find("StaticData"):GetComponent("ButtonData")[self.screen]
        Event.Publish("ButtonFocus", {focus = true, row = self.cur_row, column = self.cur_column})
    end,

    OnUpdate = function (self)
        if Input.IsKeyJustDown("space") then
            Event.Publish("ButtonPress", {row = self.cur_row, column = self.cur_column})
        elseif Input.IsKeyJustDown("s") and self.cur_row < #self.button_layout then
            Event.Publish("ButtonFocus", {focus = false, row = self.cur_row, column = self.cur_column})
            self.cur_row = self.cur_row + 1
            self.cur_column = math.min(self.cur_column,self.button_layout[self.cur_row])
            Event.Publish("ButtonFocus", {focus = true, row = self.cur_row, column = self.cur_column})
        elseif Input.IsKeyJustDown("w") and self.cur_row > 1 then
            Event.Publish("ButtonFocus", {focus = false, row = self.cur_row, column = self.cur_column})
            self.cur_row = self.cur_row - 1
            self.cur_column = math.min(self.cur_column,self.button_layout[self.cur_row])
            Event.Publish("ButtonFocus", {focus = true, row = self.cur_row, column = self.cur_column})
        elseif Input.IsKeyJustDown("d") and self.cur_column < self.button_layout[self.cur_row] then
            Event.Publish("ButtonFocus", {focus = false, row = self.cur_row, column = self.cur_column})
            self.cur_column = self.cur_column + 1
            Event.Publish("ButtonFocus", {focus = true, row = self.cur_row, column = self.cur_column})
        elseif Input.IsKeyJustDown("a") and self.cur_column > 1 then
            Event.Publish("ButtonFocus", {focus = false, row = self.cur_row, column = self.cur_column})
            self.cur_column = self.cur_column - 1
            Event.Publish("ButtonFocus", {focus = true, row = self.cur_row, column = self.cur_column})
        end
    end
}