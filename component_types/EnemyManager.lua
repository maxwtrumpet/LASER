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
            Scene.Load("Level" .. tostring(self.level + 1))
        end
    end

}