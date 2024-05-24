# SCP-3114
讓SCP-3114可以自動生成的SCP Secret Laboratory插件

# 說明
本插件為一NwApi插件
將SCP-3114加入SCP陣營或人類玩家陣營的生成池中(可在config.yml中調整)
3114的生成機率需手動調整(預設50%)


## 安裝
1. 請到 Releases 下載Spawn3114.dll 及相依的 0Harmony.dll
2. 將 Spawn3114.dll 移至 ```PluginAPI/plugins/(server_port)```, 0Harmony.dll 移至```PluginAPI/plugins/(server_port)/dependencies```


## config.yml 的預設內容

```yml

# 啟用插件嗎?Whether the plugin is enabled.
is_enabled: true
# 生成3114的機率Chance to spawn 3114.
spawn3114_chance: 0.5
# 生成3114的最少人數Minimal players to spawn 3114.
spawn3114_min_players: 1

```

##Credits
此插件之寫法架構參考自 [omgiamhungarian/PinkCandyReturns](https://github.com/omgiamhungarian/PinkCandyReturns/).
Patch 寫法參考自[拦截/注入 游戏函数实现高级操作](https://wiki.aoe.top/BepInEx/%E6%8B%A6%E6%88%AA-%E6%B3%A8%E5%85%A5-%E6%B8%B8%E6%88%8F%E5%87%BD%E6%95%B0.html) 及 [Exiled 群組](https://discord.gg/PyUkWTg)
