const fs = require('fs');

const WORKING = '.';
const DAMAGED = '#';
const UNKNOWN = '?';

function getInput(filePath) {
    return fs.readFileSync(filePath, 'utf8').split('\n').filter(Boolean);
}

function handle() {
    const input = getInput('../input/twelve.txt');
    const numberCounts = {};

    for (const line of input) {
        const unknownCount = (line.match(new RegExp('\\?', 'g')) || []).length;
        numberCounts[unknownCount] = (numberCounts[unknownCount] || 0) + 1;
    }

    for (const [n, count] of Object.entries(numberCounts)) {
        console.log(`${n} occurrences: ${count}`);
    }

    return handleStepOneDifferently();
}

function handleStepOneDifferently() {
    const input = getInput('../input/twelve.txt');
    const unknownPermDictionary = {};

    let totalCount = 0;
    for (let i = 0; i < input.length; i++) {
        const line = input[i];
        const [validConfigs, comboToCheck, localDict] = getValidConfigurations(line, unknownPermDictionary);

        for (const [key, value] of Object.entries(localDict)) {
            unknownPermDictionary[key] = value;
        }

        let lineCount = 0;
        for (const config of validConfigs) {
            if (doesLineProduceCorrectCombo(config, comboToCheck)) {
                lineCount++;
            }
        }

        console.log(`${line} ${lineCount}`);
        totalCount += lineCount;
    }

    return totalCount;
}


function getValidConfigurations(fullInputLine, unknownPermDictionary) {
    const validConfigs = new Set();
    const unknownPermutations = new Set();
    const [data, result] = fullInputLine.split(' ');
    const numberOfUnknowns = (data.match(new RegExp('\\?', 'g')) || []).length;
    const resultNumber = parseInt(result.replace(/,/g, ''));
    const unknownIndices = [...data].reduce((indices, c, i) => (c === UNKNOWN ? indices.concat(i) : indices), []);

    for (let i = 0; i <= numberOfUnknowns; i++) {
        const working = Array(i).fill(WORKING);
        const damaged = Array(numberOfUnknowns - i).fill(DAMAGED);
        unknownPermutations.add(working.join('') + damaged.join(''));
    }
    console.log({ unknownPermutations });

    const checkedPermWords = new Set();
    for (const perm of unknownPermutations) {
        const permPerms = unknownPermDictionary[perm] || permutations([...perm]);

        for (const newPerm of permPerms) {

            const permWord = newPerm;

            if (checkedPermWords.has(permWord)) {
                console.log("cacheHit");
                continue;
            }
            checkedPermWords.add(permWord);
            let newWord = data;

            for (let i = 0; i < numberOfUnknowns; i++) {
                newWord.replace('?', permWord[i]);
            }

            validConfigs.add(newWord);
        }
    }

    return [validConfigs, resultNumber, unknownPermDictionary];
}

function doesLineProduceCorrectCombo(lineData, comboToCheck) {
    let isCorrectCombo;
    let resultInt = 0;
    let brokenStringCount = 0;

    for (const char of lineData) {
        if (char === DAMAGED) {
            brokenStringCount++;
        } else if (brokenStringCount !== 0) {
            resultInt = resultInt * 10 + brokenStringCount;
            brokenStringCount = 0;
        }
    }

    if (brokenStringCount !== 0) {
        resultInt = resultInt * 10 + brokenStringCount;
    }

    isCorrectCombo = resultInt === comboToCheck;
    return isCorrectCombo;
}

if (require.main === module) {
    handle();
}

// Helper function for permutations
function* permutations(arr) {
    const length = arr.length;

    if (length <= 1) {
        yield [...arr];
        return;
    }

    for (let i = 0; i < length; i++) {
        const remaining = [...arr.slice(0, i), ...arr.slice(i + 1)];
        for (const p of permutations(remaining)) {
            yield [arr[i], ...p];
        }
    }
}

function* permute(input) {
    var i, ch;
    for (i = 0; i < input.length; i++) {
        ch = input.splice(i, 1)[0];
        usedChars.push(ch);
        if (input.length == 0) {
            permArr.push(usedChars.slice());
        }
        permute(input);
        input.splice(i, 0, ch);
        usedChars.pop();
    }
    return permArr
};

function onlyUnique(value, index, array) {
    return array.indexOf(value) === index;
}

// Helper function to add escape to RegExp (for older versions of Node.js)
RegExp.escape = function (s) {
    return s.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
};