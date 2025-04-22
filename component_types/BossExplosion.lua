BossExplosion = {

    iteration = 0,
    used_positions = {false, false, false},
    positions = {Vector2(-0.46875, 0.78125),
                 Vector2(-0.9375, -1.40625),
                 Vector2(1.25, -0.390625)},

    OnStart = function (self)
        self.sr = self.actor:GetComponent("SpriteRenderer")
        if self.iteration == 0 then
            local location = math.random(3)
            self.sr.x_position_offset = self.positions[location].x
            self.sr.y_position_offset = self.positions[location].y
            self.used_positions[location] = true
        elseif self.iteration == 1 then
            local location = math.random(2)
            if location == 1 then
                if self.used_positions[1] == true then
                    location = 2
                else
                    location = 1
                end
            else
                if self.used_positions[3] == true then
                    location = 2
                else
                    location = 3
                end
            end
            self.sr.x_position_offset = self.positions[location].x
            self.sr.y_position_offset = self.positions[location].y
            self.used_positions[location] = true
        else
            if self.used_positions[1] == false then
                self.sr.x_position_offset = self.positions[1].x
                self.sr.y_position_offset = self.positions[1].y
            elseif self.used_positions[2] == false then
                self.sr.x_position_offset = self.positions[2].x
                self.sr.y_position_offset = self.positions[2].y
            else
                self.sr.x_position_offset = self.positions[3].x
                self.sr.y_position_offset = self.positions[3].y
            end
        end

    end,

    OnDestroy = function (self)
        if self.iteration ~= 2 then
            Event.Publish("Explosion", {size = "4"})
            local explosion = Actor.Instantiate("Explosion3"):GetComponent("SpriteRenderer")
            explosion.x_position = self.sr.x_position
            explosion.y_position = self.sr.y_position
            explosion = explosion.actor:AddComponent("BossExplosion")
            explosion.iteration = self.iteration + 1
            explosion.used_positions = self.used_positions
        end
    end

}