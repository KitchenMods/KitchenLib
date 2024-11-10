const { Webhook } = require('discord-webhook-node');
const fs = require('node:fs');

let ChangelogVer = "";
let ChangelogBetaVer = "";
let DiscordURL = "";

if (process.env.CHANGELOGVER) {
    ChangelogVer = process.env.CHANGELOGVER;
}

if (process.env.CHANGELOGBETA) {
    ChangelogBetaVer = process.env.CHANGELOGBETA;
}

if (process.env.DISCORDURL) {
    DiscordURL = process.env.DISCORDURL;
}

const hook = new Webhook(DiscordURL);

const changelogpath = '../Changelogs/' + ChangelogVer + '/Github/v' + ChangelogVer + ChangelogBetaVer + '.MD';

fs.readFile(changelogpath, 'utf8', (err, data) => {
    if (err) {
      console.error(err);
      return;
    }
    if (ChangelogBetaVer == "") {
        hook.send('***KitchenLib v' + ChangelogVer + '***\nhttps://github.com/KitchenMods/KitchenLib/releases/tag/v' + ChangelogVer + '\n\n' + data + '\n\n<@&1028210506537893918>');
    } else {
        hook.send('***KitchenLib Beta v' + ChangelogVer + ChangelogBetaVer + '***\nhttps://github.com/KitchenMods/KitchenLib/releases/tag/v' + ChangelogVer + ChangelogBetaVer + '\n\n' + data + '\n\n<@&1074702337479815249>');
    }
});