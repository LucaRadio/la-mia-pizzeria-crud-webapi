﻿getPizzas();
function checkSearchInput() {
    const inputEl = document.querySelector('input');
    if (inputEl.value != '') {
        getPizzas(inputEl.value)
    } else {
        getPizzas();
    }
}

function  getPizzas(searchInput) {
     axios.get("api/Pizza/Pizzas", {
        params: {
            search: searchInput
        }
        })
         .then((resp) => {
            
            const allPizzas = resp.data;
             const rowEl = document.querySelector('.row');
             rowEl.innerHTML = '';
            allPizzas.forEach(singlePizza => {
               
                rowEl.innerHTML += `
                                        <div class="col">
                                            <div class="card bg-dark h-100">
                                            <img class="card-img-top" src ="${singlePizza.img}" alt = "Card image cap" >
                                            <div class="card-body d-flex flex-column">
                                                <h5 class="card-title text-white">${singlePizza.name}</h5>
                                                <p class="card-text text-white flex-grow-1">Ingredients: ${singlePizza.description}</p>
                                                    <div class="button-container d-flex justify-content-evenly">
                                                        <a class="btn btn-danger h-100 py-2" onclick="deletePizza(${singlePizza.id})">
                                                            <i class="fa-solid fa-trash"></i>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                            </div>
                                            `
                
            })
        })
        .catch(err => console.log(err))
}


function deletePizza(id) {
    const q = confirm('Are you sure?');

    if (q) {
        axios.delete(`/api/Pizza/Delete/${id}`)
            .then(alert('Deleted'))
            .catch(alert('Something went wrong. Try again!'))
    }
}
