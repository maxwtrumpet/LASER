ScrollerManager = {

    background_2 = {nil,nil},
    background_3 = {nil,nil},
    background_4 = {nil,nil},
    floor = {nil,nil},

    OnStart = function (self)
        local mm = Actor.Find("StaticData")
        if mm == nil then
            mm = Actor.Instantiate("StaticData")
            Actor.DontDestroy(mm)
            mm:GetComponent("MusicManager"):Init()
        else
            mm:GetComponent("MusicManager"):Reset()
        end
        local bg_1 = Actor.Find("Background_1"):GetComponents("SpriteRenderer")
        self.background_2[1] = bg_1[2]
        self.background_3[1] = bg_1[3]
        self.background_4[1] = bg_1[4]
        local bg_2 = Actor.Find("Background_2")
        bg_2:RemoveComponent(bg_2:GetComponent("SpriteRenderer"))
        bg_2 = bg_2:GetComponents("SpriteRenderer")
        self.background_2[2] = bg_2[1]
        self.background_3[2] = bg_2[2]
        self.background_4[2] = bg_2[3]
        self.background_2[2].x_position = 18
        self.background_3[2].x_position = 18
        self.background_4[2].x_position = 18
        self.floor[1] = Actor.Find("Floor_1"):GetComponent("SpriteRenderer")
        self.floor[2] = Actor.Find("Floor_2"):GetComponent("SpriteRenderer")
    end,

    OnUpdate = function (self)

        local change = self.floor[1].x_position + 0.05
        if change >= 18 then self.floor[1].x_position = change - 36
        else self.floor[1].x_position = change
        end
        change = self.floor[2].x_position + 0.05
        if change >= 18 then self.floor[2].x_position = change - 36
        else self.floor[2].x_position = change
        end

        change = self.background_2[1].x_position + 0.001
        if change >= 18 then self.background_2[1].x_position = change - 36
        else self.background_2[1].x_position = change
        end
        change = self.background_2[2].x_position + 0.001
        if change >= 18 then self.background_2[2].x_position = change - 36
        else self.background_2[2].x_position = change
        end

        change = self.background_3[1].x_position + 0.005
        if change >= 18 then self.background_3[1].x_position = change - 36
        else self.background_3[1].x_position = change
        end
        change = self.background_3[2].x_position + 0.005
        if change >= 18 then self.background_3[2].x_position = change - 36
        else self.background_3[2].x_position = change
        end

        change = self.background_4[1].x_position + 0.01
        if change >= 18 then self.background_4[1].x_position = change - 36
        else self.background_4[1].x_position = change
        end
        change = self.background_4[2].x_position + 0.01
        if change >= 18 then self.background_4[2].x_position = change - 36
        else self.background_4[2].x_position = change
        end

    end
}