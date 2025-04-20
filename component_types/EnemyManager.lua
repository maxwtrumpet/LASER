---@diagnostic disable: need-check-nil
EnemyManager = {

    level = 0,
    cur_time = 0,
    kill_points = 0,
    bonus_points = 0,
    enemy_count = 0,
    next_spawn = 0,
    prev_time = 0,

    _OnKill = function (self, event)
        self.kill_points = self.kill_points + event.points
        if event.bonus == true then
            self.kill_points = self.kill_points + self.bonus_points
            self.bonus_points = self.bonus_points + 5
            self.enemy_count = self.enemy_count - 1
        end
        self.score.text = "Score: " .. self.kill_points
        if self.cur_time >= self.ld.times[self.level] and self.enemy_count == 0 then
            local file = io.open(Application.FullPath("resources/.data/." .. self.level),"r")
            if self.kill_points > tonumber(file:read()) then
                io.close(file)
                file = io.open(Application.FullPath("resources/.data/." .. self.level),"w")
                file:write(tostring(self.kill_points))
                io.close(file)
            end
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
            Actor.Find("Gun"):DisableAll()
            Actor.Find("Player"):DisableAll()
            local beam = Actor.Find("Beam")
            if beam ~= nil then
                beam:DisableAll()
            end
            local explosions = Actor.FindAll("Explosion")
            for index, value in ipairs(explosions) do
                value:DisableAll()
            end
            self.enabled = false
        end
    end,

    OnStart = function (self)
        self.score = self.actor:GetComponent("TextRenderer")
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
        Event.Subscribe("Kill", self, self._OnKill)
        self.cur_index = {}
        for i = 1, #self.ld.enemies[self.level], 1 do
            self.cur_index[i] = 1
        end
        self.next_spawn = math.random(self.ld.spawn_range[self.level][1], self.ld.spawn_range[self.level][2])
    end,

    OnUpdate = function (self)
        self.cur_time = self.cur_time + 1
        self.progress.x_scale = math.min(3.125, self.cur_time / self.ld.times[self.level] * 3.125)
        if self.cur_time == self.next_spawn then
            local total_chance = 0
            local auto_appear = -1
            for i = 1, #self.ld.first_appearances[self.level], 1 do
                if self.cur_time > self.ld.first_appearances[self.level][i] then
                    total_chance = total_chance + self.ld.frequencies[self.level][i]
                    if self.cur_index[i] <= #self.ld.confirmed_appearances[self.level][i] and self.cur_time >= self.ld.confirmed_appearances[self.level][i] and self.prev_time < self.ld.confirmed_appearances[self.level][i] then
                        auto_appear = i
                        self.cur_index[i] = self.cur_index[i] + 1
                    end
                end
            end
            local enemy = nil
            if auto_appear == -1 then
                local type = math.random() * total_chance
                local running_total = 0
                for i = 1, #self.ld.frequencies[self.level], 1 do
                    running_total = running_total + self.ld.frequencies[self.level][i]
                    if type <= running_total then
                        enemy = self.ld.enemies[self.level][i]
                        break
                    end
                end
            else
                enemy = self.ld.enemies[self.level][auto_appear]
            end

            Actor.Instantiate(enemy)
            self.enemy_count = self.enemy_count + 1

            self.prev_time = self.cur_time
            if self.cur_time < self.ld.times[self.level] or Actor.Find("Boss") ~= nil then
                self.next_spawn = self.cur_time + math.random(self.ld.spawn_range[self.level][1], self.ld.spawn_range[self.level][2])
            end
        end
    end

}