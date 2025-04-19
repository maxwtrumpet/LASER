EnemyManager = {

    level = 0,
    cur_time = 0,

    OnStart = function (self)
        self.ld = Actor.Find("StaticData")
        if self.ld == nil then
            self.ld = Actor.Instantiate("StaticData")
            Actor.DontDestroy(self.ld)
            self.ld:GetComponent("MusicManager"):Init()
        end
        Event.Publish("Level", {level = self.level})
        self.ld = self.ld:GetComponent("LevelData")
        self.progress = self.actor:GetComponentByKey("Progress")
        self.progress.x_scale = 0
    end,

    OnUpdate = function (self)
        self.cur_time = self.cur_time + 1
        self.progress.x_scale = self.cur_time / self.ld.times[self.level] * 3.125
        if (self.cur_time == self.ld.times[self.level]) then
            Event.Publish("Win",{})
            local win = nil
            if self.level < 9 then
                Actor.Instantiate("ButtonMenu")
                Actor.Instantiate("ButtonNext"):GetComponent("ButtonNext").level = self.level
                win = Actor.Instantiate("GameMenu")
                win:GetComponent("ButtonManager").button_layout = {2}
            else
                Actor.Instantiate("ButtonMenu"):GetComponent("Rigidbody2D").x_position = 8.35
                win = Actor.Instantiate("GameMenu")
                win = win:GetComponent("ButtonManager")
                win.button_layout = {1}
                win.cur_column = 1
                win = win.actor
            end
            win:GetComponent("TextRenderer").text = "You win!"
        end
    end

}