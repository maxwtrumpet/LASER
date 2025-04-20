FastEnemy = {

    ease_factor = 0.2,
    destination = Vector2(0,0),
    cur_time = 0,
    next_move = 0,

    OnStart = function (self)

        local hm = self.actor:GetComponent("HealthManager")
        hm.damage0 = {"enemies/fast/fast_0_0","enemies/fast/fast_1_0","enemies/fast/fast_2_0",
                      "enemies/fast/fast_2_0","enemies/fast/fast_1_0","enemies/fast/fast_0_0"}
        hm.damage1 = {"enemies/fast/fast_0_0","enemies/fast/fast_1_0","enemies/fast/fast_2_0",
                      "enemies/fast/fast_2_0","enemies/fast/fast_1_0","enemies/fast/fast_0_0"}
        hm.damage2 = {"enemies/fast/fast_0_1","enemies/fast/fast_1_1","enemies/fast/fast_2_1",
                      "enemies/fast/fast_2_1","enemies/fast/fast_1_1","enemies/fast/fast_0_1"}
        hm.damage3 = {"enemies/fast/fast_0_1","enemies/fast/fast_1_1","enemies/fast/fast_2_1",
                      "enemies/fast/fast_2_1","enemies/fast/fast_1_1","enemies/fast/fast_0_1"}

        self.rb = self.actor:GetComponent("Rigidbody2D")
        local top_or_bottom = math.random(3)

        local location = Vector2(math.random() * 17 + 1, 8.5)
        if top_or_bottom == 1 then
            location = Vector2(18, math.random() * 8.5)
        end
        self.rb:SetPosition(location)
        self.destination = location
        Event.Publish("Music", {parameter = "Ostinato Slow", value = 1})
        self.next_move = math.random(45,75)
    end,

    OnUpdate = function (self)
        self.cur_time = self.cur_time + 1
        if self.cur_time == self.next_move then
            if Vector2.Length(self.destination) <= 2.5 then
                self.destination = Vector2(0,0)
            else
                local dist_diff = math.random() * 0.75 + 0.25
                local distance = Vector2.Length(self.destination) - dist_diff
                local angle = math.random() * 1.4
                self.destination = Vector2(math.cos(angle) * distance, math.sin(angle) * distance)
            end
            if Vector2.Length(self.destination) > 1 then
                self.next_move = self.cur_time + math.random(45,75)
            end
        end
    end,

    OnLateUpdate = function (self)
        local cur_pos = self.rb:GetPosition()
        self.rb:SetPosition(Vector2(cur_pos.x + (self.destination.x - cur_pos.x) * self.ease_factor,
                                    cur_pos.y + (self.destination.y - cur_pos.y) * self.ease_factor))
    end,

    OnDestroy = function (self)
        if Actor.Find("Fast") == nil then
            Event.Publish("Music", {parameter = "Ostinato Slow", value = 0})
        end
    end
}