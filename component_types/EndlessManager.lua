EndlessManager = {

    cur_time = 0,
    kill_points = 0,
    bonus_points = 0,
    next_spawn = 0,
    prev_time = 0,

    _OnMusic = function (self, event)
        if event.value == 0 then
            if (event.parameter == "Bass Low" and self.cur_time >= 21600) or
               (event.parameter == "Ostinato Slow" and self.cur_time >= 25200) or
               (event.parameter == "Ostinato Fast" and self.cur_time >= 36000) then
                Event.Publish("Music", {parameter = event.parameter, value = 1})
            end
        end
    end,

    _OnKill = function (self, event)
        self.kill_points = self.kill_points + event.points
        if event.bonus == true then
            self.kill_points = self.kill_points + self.bonus_points
            self.bonus_points = self.bonus_points + 5
        end
        self.score.text = "Score: " .. self.kill_points
    end,

    OnStart = function (self)
        self.actor:RemoveComponent(self.actor:GetComponentByKey("Meter"))
        self.actor:RemoveComponent(self.actor:GetComponentByKey("Backdrop"))
        self.actor:RemoveComponent(self.actor:GetComponentByKey("Progress"))
        self.time = self.actor:AddComponent("TextRenderer")
        self.time.size = 60
        self.time.font = "Editia"
        self.time.r = 209
        self.time.g = 209
        self.time.b = 209
        self.time.text = "Time: 0"
        self.time.x_alignment = 0.0
        self.time.x_position = 0.69
        self.time.y_position = 8.45
        self.time.sorting_order = 19
        self.score = self.actor:GetComponent("TextRenderer")
        self.ld = Actor.Find("StaticData")
        if self.ld == nil then
            self.ld = Actor.Instantiate("StaticData")
            Actor.DontDestroy(self.ld)
            self.ld:GetComponent("MusicManager"):Init()
        end
        Event.Publish("Level", {level = 10})
        self.ld = self.ld:GetComponent("LevelData")
        Event.Subscribe("Kill", self, self._OnKill)
        Event.Subscribe("Music", self, self._OnMusic)
        self.cur_index = {}
        for i = 1, #self.ld.enemies[10], 1 do
            self.cur_index[i] = 1
        end
        self.next_spawn = math.random(1, 3)
    end,

    OnUpdate = function (self)

        self.cur_time = self.cur_time + 1
        self.time.text = "Time: " .. tostring(math.floor(self.cur_time/60))
        if self.cur_time >= 36000 then
            Event.Publish("Music", {parameter = "Ostinato Fast", value = 1})
        elseif self.cur_time >= 30600 then
            Event.Publish("Music", {parameter = "Melody Low", value = 1})
        elseif self.cur_time >= 25200 then
            Event.Publish("Music", {parameter = "Ostinato Slow", value = 1})
        elseif self.cur_time >= 21600 then
            Event.Publish("Music", {parameter = "Bass Low", value = 1})
        elseif self.cur_time >= 18000 then
            Event.Publish("Music", {parameter = "Melody High", value = 1})
            Event.Publish("Music", {parameter = "Melody Low", value = 0})
        elseif self.cur_time >= 14400 then
            Event.Publish("Music", {parameter = "Drum 2", value = 1})
        elseif self.cur_time >= 12600 then
            Event.Publish("Music", {parameter = "F", value = 1})
            Event.Publish("Music", {parameter = "Ab Resolve", value = 1})
            Event.Publish("Music", {parameter = "Ab Stay", value = 0})
        elseif self.cur_time >= 10800 then
            Event.Publish("Music", {parameter = "Bb High", value = 1})
            Event.Publish("Music", {parameter = "Bb Low", value = 0})
        elseif self.cur_time >= 9000 then
            Event.Publish("Music", {parameter = "Eb", value = 1})
        elseif self.cur_time >= 7200 then
            Event.Publish("Music", {parameter = "Drum 1", value = 1})
        elseif self.cur_time >= 5400 then
            Event.Publish("Music", {parameter = "Ab Stay", value = 1})
        elseif self.cur_time >= 3600 then
            Event.Publish("Music", {parameter = "Bb Low", value = 1})
        elseif self.cur_time >= 1800 then
            Event.Publish("Music", {parameter = "Melody Low", value = 1})
        end
        if self.cur_time == self.next_spawn then
            local total_chance = 0
            local auto_appear = -1
            for i = 1, #self.ld.first_appearances[10], 1 do
                if self.cur_time >= self.ld.first_appearances[10][i] then
                    total_chance = total_chance + self.ld.frequencies[10][i]
                    if self.cur_index[i] <= #self.ld.confirmed_appearances[10][i] and
                       self.cur_time >= self.ld.confirmed_appearances[10][i][self.cur_index[i]] and
                       self.prev_time < self.ld.confirmed_appearances[10][i][self.cur_index[i]] then
                        auto_appear = i
                        self.cur_index[i] = self.cur_index[i] + 1
                    end
                end
            end
            local enemy = nil
            if auto_appear == -1 then
                local type = math.random() * total_chance
                local running_total = 0
                for i = 1, #self.ld.frequencies[10], 1 do
                    running_total = running_total + self.ld.frequencies[10][i]
                    if type <= running_total then
                        enemy = self.ld.enemies[10][i]
                        break
                    end
                end
            else
                enemy = self.ld.enemies[10][auto_appear]
            end

            Actor.Instantiate(enemy)

            self.prev_time = self.cur_time
            self.next_spawn = self.cur_time + math.random(60, math.max(60, 180 - math.floor((math.max(self.cur_time,18000) - 18000) / 150)))
        end
    end
}