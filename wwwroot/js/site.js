document.addEventListener('DOMContentLoaded', function () {
    const languageSelect = document.getElementById('languageSelect');
    const seedInput = document.getElementById('seedInput');
    const randomSeedBtn = document.getElementById('randomSeedBtn');
    const likesSlider = document.getElementById('likesSlider');
    const likesValueSpan = document.getElementById('likesValue');
    const reviewInput = document.getElementById('reviewInput');
    const tableBody = document.getElementById('books-table-body');
    const loadingIndicator = document.querySelector('.loading-indicator');
    const tableViewBtn = document.getElementById('tableViewBtn');
    const galleryViewBtn = document.getElementById('galleryViewBtn');
    const tableContainer = document.querySelector('.table-responsive');
    const mainContainer = document.querySelector('.container-fluid');
    const controlsPanel = document.querySelector('.controls-panel'); 

    let currentPage = 1;
    let isLoading = false;
    let currentView = 'table'; 

    function initializeControls() {
        const initialSeed = Math.floor(Math.random() * 10000000);
        seedInput.value = initialSeed;
        likesValueSpan.textContent = likesSlider.value;
    }

    async function fetchBooks() {
        if (isLoading) return;
        isLoading = true;
        loadingIndicator.style.display = 'block';

        const params = new URLSearchParams({
            language: languageSelect.value,
            seed: seedInput.value,
            likes: likesSlider.value,
            reviews: reviewInput.value,
            pageNumber: currentPage,
            pageSize: currentPage === 1 ? 20 : 10
        });

        try {
            const response = await fetch(`/api/books?${params.toString()}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const books = await response.json();

            if (currentView === 'table') {
                renderTable(books);
            } else {
                renderGallery(books);
            }
        } catch (error) {
            console.error('Error fetching books:', error);
        } finally {
            isLoading = false;
            loadingIndicator.style.display = 'none';
        }
    }

    function renderTable(books) {
        if (currentPage === 1) {
            tableBody.innerHTML = '';
        }

        books.forEach(book => {
            const row = document.createElement('tr');
            row.dataset.id = book.index;
            row.innerHTML = `
                <td>${book.index}</td>
                <td>${book.isbn}</td>
                <td>${book.title}</td>
                <td>${book.authors.join(', ')}</td>
                <td>${book.publisher}</td>
            `;
            row.addEventListener('click', () => toggleDetails(book, row));
            tableBody.appendChild(row);
        });
    }

    function renderGallery(books) {
        let galleryContainer = document.getElementById('books-gallery');
        if (!galleryContainer) {
            galleryContainer = document.createElement('div');
            galleryContainer.id = 'books-gallery';
            galleryContainer.classList.add('row', 'g-4', 'mt-3');
            mainContainer.appendChild(galleryContainer);
        }

        if (currentPage === 1) {
            galleryContainer.innerHTML = '';
        }

        books.forEach(book => {
            const card = document.createElement('div');
            card.classList.add('col-12', 'col-sm-6', 'col-md-4', 'col-lg-3');
            card.innerHTML = `
                <div class="card h-100">
                    <img src="${book.coverImageUrl}" class="card-img-top" alt="${book.title} cover">
                    <div class="card-body">
                        <h5 class="card-title">${book.title}</h5>
                        <p class="card-text text-muted">${book.authors.join(', ')}</p>
                    </div>
                </div>
            `;
            galleryContainer.appendChild(card);
        });
    }

    function toggleDetails(book, row) {
        const existingDetailsRow = document.querySelector('.details-row');
        const isAlreadyExpanded = row.classList.contains('expanded-row');

        if (existingDetailsRow) {
            existingDetailsRow.previousElementSibling.classList.remove('expanded-row');
            existingDetailsRow.remove();
        }

        if (isAlreadyExpanded) {
            return;
        }

        const newDetailsRow = document.createElement('tr');
        newDetailsRow.classList.add('details-row');
        newDetailsRow.innerHTML = `
            <td colspan="5">
                <div class="details-panel">
                    <div>
                        <img src="${book.coverImageUrl}" alt="${book.title} cover" class="img-fluid mb-3">
                        <div class="d-flex align-items-center">
                            <span class="badge bg-primary rounded-pill me-2">${book.likes}</span>
                            <span>likes</span>
                        </div>
                    </div>
                    <div>
                        <h5>${book.title} by ${book.authors.join(', ')}</h5>
                        <h6>Publisher: ${book.publisher}</h6>
                        <hr>
                        <h6>Reviews (${book.reviews.length})</h6>
                        ${book.reviews.length > 0
                ? book.reviews.map(r => `<div class="review-card"><strong>${r.author}:</strong> ${r.text}</div>`).join('')
                : '<p>No reviews yet.</p>'}
                    </div>
                </div>
            </td>
        `;
        row.classList.add('expanded-row');
        row.parentNode.insertBefore(newDetailsRow, row.nextSibling);
    }

    function setActiveView(view) {
        if (view === currentView) return; 

        currentView = view;
        currentPage = 1; 

        if (view === 'table') {
            tableViewBtn.classList.add('active', 'btn-primary');
            tableViewBtn.classList.remove('btn-secondary');
            galleryViewBtn.classList.remove('active', 'btn-primary');
            galleryViewBtn.classList.add('btn-secondary');

            tableContainer.classList.remove('d-none');
            galleryContainer.classList.add('d-none');
        } else {
            tableViewBtn.classList.remove('active', 'btn-primary');
            tableViewBtn.classList.add('btn-secondary');
            galleryViewBtn.classList.add('active', 'btn-primary');
            galleryViewBtn.classList.remove('btn-secondary');

            tableContainer.classList.add('d-none');
            galleryContainer.classList.remove('d-none');
        }

        fetchBooks();
    }


    function handleControlChange() {
        currentPage = 1;
        fetchBooks();
    }

    languageSelect.addEventListener('change', handleControlChange);
    seedInput.addEventListener('change', handleControlChange);
    likesSlider.addEventListener('input', () => {
        likesValueSpan.textContent = likesSlider.value;
    });
    likesSlider.addEventListener('change', handleControlChange);
    reviewInput.addEventListener('change', handleControlChange);

    randomSeedBtn.addEventListener('click', () => {
        seedInput.value = Math.floor(Math.random() * 10000000);
        handleControlChange();
    });

    tableViewBtn.addEventListener('click', () => setActiveView('table'));
    galleryViewBtn.addEventListener('click', () => setActiveView('gallery'));

    window.addEventListener('scroll', () => {
        if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 500 && !isLoading) {
            currentPage++;
            fetchBooks();
        }
    });

    initializeControls();
    fetchBooks();
});