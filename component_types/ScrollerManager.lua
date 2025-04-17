ScrollerManager = {

    background_2 = {nil,nil},
    background_3 = {nil,nil},
    background_4 = {nil,nil},
    floor = {nil,nil},

    OnStart = function (self)
        local mm = Actor.Find("StaticData")
        if mm == nil then Actor.Instantiate("StaticData"):GetComponent("MusicManager"):Init()
        else mm:GetComponent("MusicManager"):Reset()
        end
        local bg_1 = Actor.Find("Background_1"):GetComponents("SpriteRenderer")
        bg_1[1].sprite = "background/Day_1"
        bg_1[1].actor:RemoveComponent(bg_1[4])
        self.background_2[1] = bg_1[2]
        self.background_3[1] = bg_1[3]
        self.background_4[1] = bg_1[5]
        local bg_2 = Actor.Find("Background_2"):GetComponents("SpriteRenderer")
        bg_2[2].actor:RemoveComponent(bg_2[1])
        bg_2[2].actor:RemoveComponent(bg_2[4])
        self.background_2[2] = bg_2[2]
        self.background_3[2] = bg_2[3]
        self.background_4[2] = bg_2[5]
        self.floor[1] = Actor.Find("Floor_1"):GetComponent("Rigidbody2D")
        self.floor[2] = Actor.Find("Floor_2"):GetComponent("Rigidbody2D")
    end,

    OnUpdate = function (self)

        local change = self.floor[1]:GetPosition().x + 0.05
        if change >= 18 then self.floor[1]:SetPosition(Vector2(change - 36,0))
        else self.floor[1]:SetPosition(Vector2(change,0))
        end
        change = self.floor[2]:GetPosition().x + 0.05
        if change >= 18 then self.floor[2]:SetPosition(Vector2(change - 36,0))
        else self.floor[2]:SetPosition(Vector2(change,0))
        end

        change = self.background_2[1].x_position_offset + 0.001
        if change >= 18 then self.background_2[1].x_position_offset = change - 36
        else self.background_2[1].x_position_offset = change
        end
        change = self.background_2[2].x_position_offset + 0.001
        if change >= 18 then self.background_2[2].x_position_offset = change - 36
        else self.background_2[2].x_position_offset = change
        end

        change = self.background_3[1].x_position_offset + 0.005
        if change >= 18 then self.background_3[1].x_position_offset = change - 36
        else self.background_3[1].x_position_offset = change
        end
        change = self.background_3[2].x_position_offset + 0.005
        if change >= 18 then self.background_3[2].x_position_offset = change - 36
        else self.background_3[2].x_position_offset = change
        end

        change = self.background_4[1].x_position_offset + 0.01
        if change >= 18 then self.background_4[1].x_position_offset = change - 36
        else self.background_4[1].x_position_offset = change
        end
        change = self.background_4[2].x_position_offset + 0.01
        if change >= 18 then self.background_4[2].x_position_offset = change - 36
        else self.background_4[2].x_position_offset = change
        end

    end
}