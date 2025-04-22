---@diagnostic disable: need-check-nil
HealthManager = {

    health = 4,
    points = 1,
    explosion = "0",
    boss = false,
    countdown = -1,
    damage0 = {},
    damage1 = {},
    damage2 = {},
    damage3 = {},

    OnStart = function (self)
        self.interacted = {}
        self.cur_health = self.health
        if self.points ~= 1 then
            self.am = self.actor:GetComponent("Animator")
            self.am.sprite = self.actor:GetComponent("SpriteRenderer")
            self.am.frames = self.damage0
        end
    end,

    OnUpdate = function (self)
        if self.countdown ~= -1 then
            self.countdown = self.countdown - 1
            if self.countdown == 0 then
                Actor.Destroy(self.actor)
            end
        end
    end,

    OnLateUpdate = function (self)
        if self.cur_health <= 0 and self.countdown == -1 then
            local exp = Actor.Instantiate("Explosion" .. tostring(math.min(3,tonumber(self.explosion)))):GetComponent("SpriteRenderer")
            local position = self.actor:GetComponent("Rigidbody2D"):GetPosition()
            exp.x_position = position.x
            exp.y_position = position.y
            if self.points == 1000 then
                exp.actor:AddComponent("BossExplosion")
                self.actor:GetComponent("BossEnemy").enabled = false
                self.actor:GetComponent("Rigidbody2D"):SetVelocity(Vector2(0,0))
                self.actor:GetComponent("Animator").enabled = false
                self.actor:GetComponent("SpriteRenderer").sprite = "enemies/boss/boss_0_3"
                self.countdown = 90
            else
                Actor.Destroy(self.actor)
            end
            Event.Publish("Explosion", {size = self.explosion})
            Event.Publish("Kill", {points = self.points, bonus = self.points > 1})
        elseif self.cur_health <= self.health / 4 then
            self.am.frames = self.damage3
        elseif self.cur_health <= self.health / 2 then
            self.am.frames = self.damage2
        elseif self.cur_health <= 3 * self.health / 4 then
            self.am.frames = self.damage1
        end
    end,

    OnTriggerEnter2D = function(self, contact)
        if contact.other:GetName() == "Player" then
            Event.Publish("Explosion", {size = "4"})
            Event.Publish("Lose", {})
            Actor.Instantiate("ButtonMenu")
            Actor.Instantiate("ButtonRetry")
            local lose = Actor.Instantiate("GameMenu")
            lose:GetComponent("ButtonManager").button_layout = {2}
            lose:GetComponent("TextRenderer").text = "Game over..."
            Actor.Find("Floor"):GetComponent("GeneralManager").enabled = false
            local ui = Actor.Find("UI")
            local manager = ui:GetComponent("EndlessManager")
            if manager == nil then
                ui:GetComponent("EnemyManager").enabled = false
            else
                manager.enabled = false
                local file = io.open(Application.FullPath("resources/.data/10"),"r")
                local time = math.max(math.floor(manager.cur_time/60),tonumber(file:read()))
                local points = math.max(manager.kill_points,tonumber(file:read()))
                io.close(file)
                file = io.open(Application.FullPath("resources/.data/10"),"w")
                file:write(tostring(time) .. "\n" .. tostring(points))
                io.close(file)
            end
            Actor.Find("Gun"):DisableAll()
            Actor.Find("Player"):DisableAll()
            local beam = Actor.Find("Beam")
            if beam ~= nil then
                beam:DisableAll()
            end
            local basic = Actor.FindAll("Basic")
            for index, value in ipairs(basic) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local fast = Actor.FindAll("Fast")
            for index, value in ipairs(fast) do
                value:DisableAll()
            end
            local egg = Actor.FindAll("Egg")
            for index, value in ipairs(egg) do
                value:DisableAll()
            end
            local gnat = Actor.FindAll("Gnat")
            for index, value in ipairs(gnat) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local kamikaze = Actor.FindAll("Kamikaze")
            for index, value in ipairs(kamikaze) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local smoke = Actor.FindAll("Smoke")
            for index, value in ipairs(smoke) do
                value:DisableAll()
            end
            local boss = Actor.FindAll("Boss")
            for index, value in ipairs(boss) do
                value:DisableAll()
                value:GetComponentByKey("XManager"):OnDisable()
            end
            local explosions = Actor.FindAll("Explosion")
            for index, value in ipairs(explosions) do
                value:DisableAll()
            end
            self.enabled = false
        elseif contact.other:GetName() == "Beam" then
            local bm = contact.other:GetComponent("BeamManager")
            if self.interacted[tostring(bm.identifier)] == nil then
                self.interacted[tostring(bm.identifier)] = true
                self.cur_health = self.cur_health - bm.damage
            end
        end
    end

}