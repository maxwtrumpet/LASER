GunController = {

    shoot_ease_factor = 0.2,
    charge_ease_factor = 0.025,
    cur_charge = 1.0,
    cannon = "event:/effects/cannon",
    blank_vector = Vector3(0,0,0),
    previous_scroll = 0,
    beam_counter = 1,

    OnStart = function (self)
        self.rb2d = self.actor:GetComponent("Rigidbody2D")
        self.meters = {}
        self.charges = {}
        self.meters[1] = self.actor:GetComponentByKey("LeftMeter")
        self.meters[2] = self.actor:GetComponentByKey("RightMeter")
        self.charges[1] = self.actor:GetComponentByKey("LeftCharge")
        self.charges[2] = self.actor:GetComponentByKey("RightCharge")
        Audio.PlayEvent(self.cannon, 0.5, self.blank_vector, self.blank_vector, false)
        self.am = self.actor:GetComponent("Animator")
        self.am.frames = {"player/beam_1", "player/beam_2", "player/beam_3", "player/beam_4"}
        self.gun = self.actor:GetComponentByKey("Gun")
        self.am.sprite = self.gun
    end,

    OnUpdate = function (self)
        if Input.IsKeyDown("w") or Input.IsKeyDown("d") then
            local current_angle = self.rb2d:GetRotation()
            local desired_angle = nil
            local final_angle = current_angle
            if Input.IsKeyDown("w") then
                desired_angle = 90
            else
                desired_angle = 0
            end
            local factor = (0.005 + self.cur_charge / 100) * 180 / math.pi
            if math.abs(current_angle - desired_angle) > factor then
                if current_angle < desired_angle then
                    final_angle = current_angle + factor
                else
                    final_angle = current_angle - factor
                end
            end
            if final_angle > 90 then
                final_angle = 90
            elseif final_angle < 0 then
                final_angle = 0
            end
            self.rb2d:SetRotation(final_angle)
        end
        local scroll = Input.GetMouseScrollDelta()
        if scroll == 0 and self.previous_scroll > 0.05 then
            scroll = self.previous_scroll / 2
        end
        if scroll > 0 then
            self.cur_charge = self.cur_charge - 0.008
            if self.cur_charge < 0 then
                self.cur_charge = 0
            end
            if self.cur_charge == 0 then
                self.meters[1].sprite = "player/ray_4"
                self.meters[2].sprite = "player/ray_4"
            elseif self.cur_charge < 0.375 then
                self.meters[1].sprite = "player/ray_3"
                self.meters[2].sprite = "player/ray_3"
            elseif self.cur_charge < 0.625 then
                self.meters[1].sprite = "player/ray_2"
                self.meters[2].sprite = "player/ray_2"
            elseif self.cur_charge < 0.75 then
                self.meters[1].sprite = "player/ray_1"
                self.meters[2].sprite = "player/ray_1"
            end
        end
        self.previous_scroll = scroll
        if self.cur_charge < 0.75 and Input.IsKeyJustDown("space") then

            local cur_beam = Actor.Instantiate("Beam"):GetComponent("Rigidbody2D")
            local angle = self.rb2d:GetRotation()
            cur_beam:SetRotation(angle)
            angle = angle * math.pi / 180
            cur_beam:SetPosition(Vector2(12.25 * math.cos(angle),12.25 * math.sin(angle)))
            cur_beam.actor:GetComponent("SpriteRenderer").y_scale = self.meters[1].y_position_offset * 3.125

            cur_beam = cur_beam.actor:GetComponent("BeamManager")
            if self.cur_charge == 0 then
                cur_beam.damage = 7
            elseif self.cur_charge < 0.375 then
                cur_beam.damage = 4
            elseif self.cur_charge < 0.625 then
                cur_beam.damage = 2
            else
                cur_beam.damage = 1
            end
            cur_beam.identifier = self.beam_counter
            self.beam_counter = self.beam_counter + 1

            local main_idx = "0"
            if self.cur_charge == 0 then
                main_idx = "3"
            elseif self.cur_charge < 0.375 then
                main_idx = "2"
            elseif self.cur_charge < 0.625 then
                main_idx = "1"
            end
            Event.Publish("Gun", {main = main_idx, random = tostring(math.random(3)-1)})

            self.cur_charge = 1
            self.meters[1].sprite = "player/ray_0"
            self.meters[2].sprite = "player/ray_0"
        end
    end,

    OnLateUpdate = function (self)

        local new_size = 0.05
        if self.cur_charge < 0.75 then
            new_size = (0.75 - self.cur_charge) / 0.75 * 0.95 + 0.05
        end
        local real_size = self.meters[1].y_position_offset + (new_size - self.meters[1].y_position_offset) * self.shoot_ease_factor
        self.meters[1].y_position_offset = real_size
        self.meters[2].y_position_offset = -real_size
        self.charges[1].y_position_offset = real_size
        self.charges[2].y_position_offset = -real_size

        local ease_tint = (1 - self.cur_charge) * 3.125
        if ease_tint < self.charges[1].x_scale then
            ease_tint = self.charges[1].x_scale + (ease_tint - self.charges[1].x_scale) * self.charge_ease_factor
        end
        self.charges[1].x_scale = ease_tint
        self.charges[2].x_scale = ease_tint

        Audio.SetEventParameter(self.cannon, "power", 1 - self.cur_charge)

        if self.cur_charge == 0 then
            self.am.enabled = true
            self.am.speed = 2.4
        elseif self.cur_charge < 0.375 then
            self.am.enabled = true
            self.am.speed = 1.85
        elseif self.cur_charge < 0.625 then
            self.am.enabled = true
            self.am.speed = 1.3
        elseif self.cur_charge < 0.75 then
            self.am.enabled = true
            self.am.speed = 0.75
        else
            self.am.enabled = false
            self.gun.sprite = "player/beam_0"
        end

    end

}