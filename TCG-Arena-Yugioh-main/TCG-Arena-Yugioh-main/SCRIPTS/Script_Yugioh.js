const fs = require('fs');

// Fonction pour lire et modifier le fichier JSON
function modifyJsonFile(inputFilePath, outputFilePath) {
    // Lire le fichier JSON
    fs.readFile(inputFilePath, 'utf8', (err, data) => {
        if (err) {
            console.error('Erreur lors de la lecture du fichier JSON:', err);
            return;
        }

        try {
            // Convertir les données JSON en objet
            const jsonObject = JSON.parse(data);

            let result = {}
            let errorCount = 0

            jsonObject.data.forEach((c) => {
                let newCard = {}
 
                const cardId = c.id;
                let type = "Monster"
                if (c.type.toLowerCase().includes("spell")) {
                    type = "Spell"
                } else if (c.type.toLowerCase().includes("trap")) {
                    type = "Trap"
                } else if (c.type.toLowerCase().includes("skill")) {
                    type = "Skill"
                }
                
                newCard = {
                    id: cardId,
                    face: {
                        front: {
                            name: c.name,
                            type: type,
                            cost: 0,
                            image: c.card_images[0].image_url,
                            isHorizontal: false
                        }
                    },
                    name: c.name,
                    type: type,
                    "Type line": c.humanReadableCardType,
                    cost: 0,
                    rarity: c.set_rarity,
                    race: c.race,
                    archetype: c.archetype,
                    atk: c.atk,
                    def: c.def,
                    level: c.level,
                    attribute: c.attribute
                }
                if (result[cardId]) {
                    console.log("Error: card " + c.name + " already exist with id " + cardId)
                    errorCount += 1
                } else {
                    result[cardId] = newCard
                    //console.log("Error: card " + c.fullName + " has no color")
                    //errorCount += 1
                }
            })


            // Sauvegarder le nouvel objet JSON dans un nouveau fichier
            fs.writeFile(outputFilePath, JSON.stringify(result, null, 2), 'utf8', (err) => {
                if (err) {
                    console.error('Erreur lors de l\'écriture du fichier JSON:', err);
                } else {
                    console.log('Le fichier JSON a été modifié et sauvegardé sous', outputFilePath);
                    console.log("Saved " + Object.keys(result).length + " cards on the " + jsonObject.data.length + " expected. Failed for " + errorCount + " cards")
                }
            });
        } catch (e) {
            console.error('Erreur lors du traitement du fichier JSON:', e);
        }
    });
}

// Utilisation de la fonction
const inputFilePath = 'db.json';  // Chemin vers le fichier JSON d'entrée
const outputFilePath = 'YugiohCards.json';  // Chemin vers le fichier JSON de sortie
modifyJsonFile(inputFilePath, outputFilePath);