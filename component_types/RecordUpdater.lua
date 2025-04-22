---@diagnostic disable: need-check-nil
RecordUpdater = {
    OnStart = function (self)
        for i = 1, 9, 1 do
            local level = io.open(Application.FullPath("resources/.data/" .. i),"r")
            self.actor:GetComponentByKey("Score" .. i).text = level:read()
            io.close(level)
        end
        local endless = io.open(Application.FullPath("resources/.data/10"),"r")
        self.actor:GetComponentByKey("EndlessTime").text = endless:read() .. " seconds"
        self.actor:GetComponentByKey("EndlessScore").text = endless:read() .. " points"
        io.close(endless)
    end
}