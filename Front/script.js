function generateDataModel() {
    // let dto = {}; // Data Transfer Object
    // dto.name = document.getElementById("text-datamodel-name").value;
    // dto.tag = document.getElementById("text-datamodel-tag").value;
    // dto.time = document.getElementById("picker-starttime").value;

    // let fieldNames = document.getElementsByClassName("text-field-name");
    // let fieldTypes = document.getElementsByClassName("slcFieldType");
    // dto.fields = [];
    // for (let i = 0; i < fieldNames.length; i++) {
    //     dto.fields[i] = {
    //         fieldName: fieldNames[i].value,
    //         fieldType: fieldTypes[i].value
    //     };
    // }

    callApi();
}

function addFieldBox() {

    const fieldTypeBoxes = document.getElementsByClassName("field");
    const boxesCnt = fieldTypeBoxes.length;
    const lastFieldTypeBox = fieldTypeBoxes[boxesCnt - 1];

    const newWrapDiv = document.createElement("tbody");
    newWrapDiv.className = "field";

    const newRow1 = document.createElement("tr");

    const newTdLbl1 = document.createElement("td");
    newRow1.appendChild(newTdLbl1);

    const newTdInp = document.createElement("td");
    newRow1.appendChild(newTdInp);

    const newNameLabel = document.createElement("label");
    newNameLabel.innerHTML = "Name:";
    newNameLabel.for = `text-field-name-${boxesCnt}`;
    newTdLbl1.appendChild(newNameLabel);

    const newNameInput = document.createElement("input");
    newNameInput.type = "text";
    newNameInput.id = `text-field-name-${boxesCnt}`;
    newNameInput.className = "text-field-name";
    newTdInp.appendChild(newNameInput);

    const newRow2 = document.createElement("tr");

    const newTdLbl2 = document.createElement("td");
    newRow2.appendChild(newTdLbl2);

    const newTdSlc = document.createElement("td");
    newRow2.appendChild(newTdSlc);

    const newTypeName = document.createElement("label");
    newTypeName.innerHTML = "Type of Data:";
    newTypeName.for = `text-datatype-${boxesCnt}`;
    newTdLbl2.appendChild(newTypeName);

    var select = document.createElement("select");
    select.setAttribute("id", `text-datatype-${boxesCnt}`);
    select.setAttribute("class", "slcFieldType");

    var options = ["string", "int", "double", "bool"];
    for (var i = 0; i < options.length; i++) {
        var option = document.createElement("option");
        option.value = options[i];
        option.textContent = options[i];
        select.appendChild(option);
    }
    newTdSlc.appendChild(select);

    const newRow3 = document.createElement("tr");

    const newTdLbl3 = document.createElement("td");
    newRow3.appendChild(newTdLbl3);

    const newTdRange = document.createElement("td");
    newRow3.appendChild(newTdRange);

    const newValRangeLbl = document.createElement("label");
    newValRangeLbl.innerHTML = "Value Range:";
    newTdLbl3.appendChild(newValRangeLbl);

    const newMinValLbl = document.createElement("label");
    newMinValLbl.innerHTML = "From:";
    newTdRange.appendChild(newMinValLbl);

    const newMinValInput = document.createElement("input");
    newMinValInput.type = "number";
    newMinValInput.id = `num-minvalue-${boxesCnt}`;
    newTdRange.appendChild(newMinValInput);
    newTdRange.appendChild(document.createElement("br"));

    const newMaxValLbl = document.createElement("label");
    newMaxValLbl.innerHTML = "To:";
    newTdRange.appendChild(newMaxValLbl);


    const newMaxValInput = document.createElement("input");
    newMaxValInput.type = "number";
    newMaxValInput.id = `num-maxvalue-${boxesCnt}`;
    newTdRange.appendChild(newMaxValInput);


    newWrapDiv.appendChild(document.createElement("br"));
    newWrapDiv.appendChild(newRow1);
    newWrapDiv.appendChild(newRow2);
    newWrapDiv.appendChild(newRow3);
    lastFieldTypeBox.insertAdjacentElement("afterend", newWrapDiv);

}

document.addEventListener("DOMContentLoaded", function (elm, ev) {
    document.getElementById("btn-addfield").addEventListener("click", addFieldBox);
    document.getElementById("btn-removefield").addEventListener("click", removeLastField);
    document.getElementById("btn-gendata").addEventListener("click", generateDataModel);
    document.getElementById("btn-savedata").addEventListener("click", displayDataPlus);
});

function removeLastField() {
    const fieldTypeBoxes = document.getElementsByClassName("field");
    const boxesCnt = fieldTypeBoxes.length;
    const lastFieldTypeBox = fieldTypeBoxes[boxesCnt - 1];

    if (boxesCnt > 1) { 
        lastFieldTypeBox.remove();
    }
    else {
        alert("at least one field box must be available");
    }
}

// function callApi() {
//     // URL for the POST request
//     const url = 'http://localhost:5219/api/simulator/saveDataModel';

//     // Data to be sent in the request body (can be a JSON object, FormData, etc.)
//     const dataModelName = document.getElementById("text-datamodel-name").value;
//     const dataModelTag = document.getElementById("text-datamodel-tag").value;
//     const StartTime = document.getElementById("picker-starttime").value;

//     const fields = [];
//     const fieldCnt = document.getElementsByClassName("field").length;

//     for (let i = 0; i < fieldCnt; i++) {
//         let tmp = {
//             fieldName: document.getElementById(`text-field-name-${i}`).value,
//             fieldType: document.getElementById(`text-datatype-${i}`).value,
//             minValue: document.getElementById(`num-minvalue-${i}`).value,
//             maxValue: document.getElementById(`num-maxvalue-${i}`).value
//         };
//         fields.push(tmp);
//     }

//     const postData = {
//         dataModelName, dataModelTag, StartTime, fields
//     };

//     // Options for the fetch request
//     const options = {
//         method: 'POST',
//         headers: {
//             'Content-Type': 'application/json', // Set the content type based on the data being sent
//             // Add any other headers if needed
//         },
//         body: JSON.stringify(postData) // Convert the data to a JSON string
//     };
//     console.log(postData);
//     // Make the fetch request
//     // fetch(url, options)
//     //     .then(response => {
//     //         // Check if the request was successful (status code 2xx)
//     //         if (response.ok) {
//     //             return "Success"; 
//     //         } else {
//     //             throw new Error('Request failed');
//     //         }
//     //     })
//     //     .then(data => {
//     //         // Handle the response data
//     //         console.log(data);
//     //     })
//     //     .catch(error => {
//     //         // Handle errors during the fetch request
//     //         console.error('Error:', error);
//     //     });

// }
function callApi() {
    const generateTime = document.getElementById("num-simlen").value;
    const timeInterval = document.getElementById("num-interval").value;

    const url = `http://localhost:5219/api/simulator/generateData/${generateTime}/${timeInterval}`;

    const dataModelName = document.getElementById("text-datamodel-name").value;
    const dataModelTag = document.getElementById("text-datamodel-tag").value;
    const startTime = document.getElementById("picker-starttime").value;

    const fields = [];
    const fieldCnt = document.getElementsByClassName("field").length;

    for (let i = 0; i < fieldCnt; i++) {
        let tmp = {
            fieldName: document.getElementById(`text-field-name-${i}`).value,
            dataType: document.getElementById(`text-datatype-${i}`).value,
            minValue: document.getElementById(`num-minvalue-${i}`).value,
            maxValue: document.getElementById(`num-maxvalue-${i}`).value
        };
        fields.push(tmp);
    }

    const postData = {
        dataModelName, dataModelTag, startTime, fields,
    };

    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(postData)
    };


    fetch(url, options)
        .then(response => response)
        .then(data => {
            console.log(data);
            displayData(dataModelName);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
function displayData(measurement) {
    const dataList = document.getElementById("data-list");
    dataList.innerHTML = '';

    fetch(`http://localhost:5219/api/simulator/queryData/${measurement}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(result => {
            console.log(result);
            result.forEach(item => {
                console.log(item);
                const listItem = document.createElement("li");
                listItem.textContent = `Measurement: ${item.values._measurement}, Value: ${item.values._value}, Tag: ${item.values.tag}`;
                dataList.appendChild(listItem);
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function displayDataPlus() {
    const tmp = document.getElementById("txt-modelname").value;
    // console.log(tmp);
    displayData(tmp);
}
