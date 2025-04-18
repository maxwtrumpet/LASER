ButtonLevels = {

    focus = nil,

    _OnPress = function (self, event)
        if event.row == self.focus.row and event.column == self.focus.column then
            for i = 1, 9, 1 do
                local cur_button = Actor.Instantiate("ButtonLevel")
                cur_button:GetComponent("Rigidbody2D"):SetPosition(Vector2((((i - 1) % 3) - 1) * 5.5, (2 - math.ceil(i / 3)) * 2 + 1.5))
                cur_button:GetComponent("TextRenderer").text = i
                cur_button = cur_button:GetComponent("ButtonFocus")
                cur_button.row = math.ceil(i / 3)
                cur_button.column = ((i-1) % 3) + 1
            end
            Actor.Instantiate("ButtonReturn"):GetComponent("ButtonManager").button_layout = {3,3,3,1}
            Actor.Destroy(Actor.Find("Logo"))
            Actor.Destroy(Actor.Find("ButtonEndless"))
            Actor.Destroy(Actor.Find("ButtonRecords"))
            Actor.Destroy(Actor.Find("ButtonQuit"))
            Actor.Destroy(Actor.Find("ButtonControls"))
            Actor.Destroy(Actor.Find("ButtonCredits"))
            Actor.Destroy(self.actor)
        end
    end,

    OnStart = function (self)
        self.focus = self.actor:GetComponent("ButtonFocus")
        Event.Subscribe("ButtonPress", self, self._OnPress)
    end

}