ButtonBack = {

    row = 0,
    column = 0,

    OnUpdate = function (self)
        if Input.IsKeyJustDown("space") or Input.IsButtonJustDown("button down") then
            Event.Publish("ButtonPress",{})
            Actor.Instantiate("ButtonLevels")
            Actor.Instantiate("ButtonEndless")
            Actor.Instantiate("ButtonRecords")
            Actor.Instantiate("ButtonQuit")
            Actor.Instantiate("ButtonControls")
            Actor.Instantiate("ButtonCredits")
            local bm = Actor.Instantiate("Logo"):GetComponent("ButtonManager")
            bm.cur_row = self.row
            bm.cur_column = self.column
            Actor.Destroy(self.actor)
        end
    end

}