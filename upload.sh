#!/bin/sh
set -e

echo "Uploading to steam workshop"

cat << EOF > $PWD/workshop.vdf
"workshopitem"
{
  "appid" "${INPUT_APPID}"
  "contentfolder" "${GITHUB_WORKSPACE}/${INPUT_PATH}"
  "changenote" "Automatic upload from Github Actions"
  "publishedfileid" "${INPUT_ITEMID}"
}
EOF

echo "$(cat $PWD/workshop.vdf)"

if [[ -z "${STEAM_CODE}" ]]; then
  steamcmd +@ShutdownOnFailedCommand 1 +login "${STEAM_USERNAME}" "${STEAM_PASSWORD}" +workshop_build_item ${PWD}/workshop.vdf +quit
else
  steamcmd +@ShutdownOnFailedCommand 1 +login "${STEAM_USERNAME}" "${STEAM_PASSWORD}" ${STEAM_CODE} +workshop_build_item ${PWD}/workshop.vdf +quit
fi
