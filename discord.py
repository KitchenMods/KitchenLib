from discordwebhook import Discord
import os

with open('KitchenLib/Changelogs/'+os.environ['CHANGELOGVER']+'/Github/v'+os.environ['CHANGELOGVER']+os.environ['CHANGELOGBETA']+'.MD', 'r', encoding='utf-8-sig') as file:
    data = file.read()

discord = Discord(url=os.environ['DISCORDURL'])
if (os.environ['CHANGELOGBETA'] == ""):
    discord.post(content='***KitchenLib v'+os.environ['CHANGELOGVER']+'***\nhttps://github.com/KitchenMods/KitchenLib/releases/tag/v'+os.environ['CHANGELOGVER']+'\n\n'+data+'\n\n<@&1028210506537893918>')
else:
    discord.post(content='***KitchenLib Beta v'+os.environ['CHANGELOGVER']+os.environ['CHANGELOGBETA']+'***\nhttps://github.com/KitchenMods/KitchenLib/releases/tag/v'+os.environ['CHANGELOGVER']+os.environ['CHANGELOGBETA']+'\n\n'+data+'\n\n<@&1074702337479815249>')

