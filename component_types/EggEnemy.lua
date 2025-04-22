EggEnemy = {

    remaining_time = 600,

    OnStart = function (self)
        local hm = self.actor:GetComponent("HealthManager")
        hm.damage0 = {"enemies/egg/egg_0_0","enemies/egg/egg_1_0","enemies/egg/egg_2_0",
                      "enemies/egg/egg_2_0","enemies/egg/egg_1_0","enemies/egg/egg_0_0"}
        hm.damage1 = {"enemies/egg/egg_0_1","enemies/egg/egg_1_1","enemies/egg/egg_2_1",
                      "enemies/egg/egg_2_1","enemies/egg/egg_1_1","enemies/egg/egg_0_1"}
        hm.damage2 = {"enemies/egg/egg_0_2","enemies/egg/egg_1_2","enemies/egg/egg_2_2",
                      "enemies/egg/egg_2_2","enemies/egg/egg_1_2","enemies/egg/egg_0_2"}
        hm.damage3 = {"enemies/egg/egg_0_3","enemies/egg/egg_1_3","enemies/egg/egg_2_3",
                      "enemies/egg/egg_2_3","enemies/egg/egg_1_3","enemies/egg/egg_0_3"}
        self.am = self.actor:GetComponent("Animator")
        self.rb2d = self.actor:GetComponent("Rigidbody2D")
        local top_or_bottom = math.random() * 19
        if top_or_bottom <= 5 then
            local y_coord = math.random() * 5 + 1.5
            self.rb2d:SetPosition(Vector2(18,y_coord))
            self.actor:GetComponent("MoveWithEase").destination = Vector2(16,y_coord)
        else
            local x_coord = math.random() * 14 + 2
            self.rb2d:SetPosition(Vector2(x_coord,8.5))
            self.actor:GetComponent("MoveWithEase").destination = Vector2(x_coord,6.5)
        end
        Event.Publish("Music", {parameter = "Bass Low", value = 1})
    end,

    OnUpdate = function (self)
        self.remaining_time = self.remaining_time - 1
        if self.remaining_time <= 0 and -self.remaining_time % 6 == 0 then
            Actor.Instantiate("GnatEnemy"):GetComponent("Rigidbody2D"):SetPosition(self.rb2d:GetPosition())
        elseif self.remaining_time > 0 then
            self.am.speed = (600 - self.remaining_time) / 600 * 1.5 + 0.5
        end
    end,

    OnDestroy = function (self)
        if Actor.Find("Egg") == nil then
            Event.Publish("Music", {parameter = "Bass Low", value = 0})
        end
    end

}